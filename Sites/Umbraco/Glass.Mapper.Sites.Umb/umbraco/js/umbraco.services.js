/*! umbraco - v7.0.0-Beta - 2013-11-21
 * https://github.com/umbraco/umbraco-cms/tree/7.0.0
 * Copyright (c) 2013 Umbraco HQ;
 * Licensed MIT
 */

(function() { 

angular.module("umbraco.services", ["umbraco.security", "umbraco.resources"]);

/**
 * @ngdoc service
 * @name umbraco.services.angularHelper
 * @function
 *
 * @description
 * Some angular helper/extension methods
 */
function angularHelper($log, $q) {
    return {

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#rejectedPromise
         * @methodOf umbraco.services.angularHelper
         * @function
         *
         * @description
         * In some situations we need to return a promise as a rejection, normally based on invalid data. This
         * is a wrapper to do that so we can save on writing a bit of code.
         *
         * @param {object} objReject The object to send back with the promise rejection
         */
        rejectedPromise: function (objReject) {
            var deferred = $q.defer();
            //return an error object including the error message for UI
            deferred.reject(objReject);
            return deferred.promise;
        },

        /**
         * @ngdoc function
         * @name safeApply
         * @methodOf umbraco.services.angularHelper
         * @function
         *
         * @description
         * This checks if a digest/apply is already occuring, if not it will force an apply call
         */
        safeApply: function (scope, fn) {
            if (scope.$$phase || scope.$root.$$phase) {
                if (angular.isFunction(fn)) {
                    fn();
                }
            }
            else {
                if (angular.isFunction(fn)) {
                    scope.$apply(fn);
                }
                else {
                    scope.$apply();
                }
            }
        },

        /**
         * @ngdoc function
         * @name getCurrentForm
         * @methodOf umbraco.services.angularHelper
         * @function
         *
         * @description
         * Returns the current form object applied to the scope or null if one is not found
         */
        getCurrentForm: function (scope) {

            //NOTE: There isn't a way in angular to get a reference to the current form object since the form object
            // is just defined as a property of the scope when it is named but you'll always need to know the name which
            // isn't very convenient. If we want to watch for validation changes we need to get a form reference.
            // The way that we detect the form object is a bit hackerific in that we detect all of the required properties 
            // that exist on a form object.
            //
            //The other way to do it in a directive is to require "^form", but in a controller the only other way to do it
            // is to inject the $element object and use: $element.inheritedData('$formController');

            var form = null;
            //var requiredFormProps = ["$error", "$name", "$dirty", "$pristine", "$valid", "$invalid", "$addControl", "$removeControl", "$setValidity", "$setDirty"];
            var requiredFormProps = ["$addControl", "$removeControl", "$setValidity", "$setDirty", "$setPristine"];

            // a method to check that the collection of object prop names contains the property name expected
            function propertyExists(objectPropNames) {
                //ensure that every required property name exists on the current scope property
                return _.every(requiredFormProps, function (item) {

                    return _.contains(objectPropNames, item);
                });
            }

            for (var p in scope) {

                if (_.isObject(scope[p]) && p !== "this" && p.substr(0, 1) !== "$") {
                    //get the keys of the property names for the current property
                    var props = _.keys(scope[p]);
                    //if the length isn't correct, try the next prop
                    if (props.length < requiredFormProps.length) {
                        continue;
                    }

                    //ensure that every required property name exists on the current scope property
                    var containProperty = propertyExists(props);

                    if (containProperty) {
                        form = scope[p];
                        break;
                    }
                }
            }

            return form;
        },

        /**
         * @ngdoc function
         * @name validateHasForm
         * @methodOf umbraco.services.angularHelper
         * @function
         *
         * @description
         * This will validate that the current scope has an assigned form object, if it doesn't an exception is thrown, if
         * it does we return the form object.
         */
        getRequiredCurrentForm: function (scope) {
            var currentForm = this.getCurrentForm(scope);
            if (!currentForm || !currentForm.$name) {
                throw "The current scope requires a current form object (or ng-form) with a name assigned to it";
            }
            return currentForm;
        },

        /**
         * @ngdoc function
         * @name getNullForm
         * @methodOf umbraco.services.angularHelper
         * @function
         *
         * @description
         * Returns a null angular FormController, mostly for use in unit tests
         *      NOTE: This is actually the same construct as angular uses internally for creating a null form but they don't expose
         *          any of this publicly to us, so we need to create our own.
         *
         * @param {string} formName The form name to assign
         */
        getNullForm: function (formName) {
            return {
                $addControl: angular.noop,
                $removeControl: angular.noop,
                $setValidity: angular.noop,
                $setDirty: angular.noop,
                $setPristine: angular.noop,
                $name: formName
                //NOTE: we don't include the 'properties', just the methods.
            };
        }
    };
}
angular.module('umbraco.services').factory('angularHelper', angularHelper);
/**
 * @ngdoc service
 * @name umbraco.services.appState
 * @function
 *
 * @description
 * Tracks the various application state variables when working in the back office, raises events when state changes.
 *
 * ##Samples
 *
 * ####Subscribe to global state changes:
 * 
 * <pre>
  *    scope.showTree = appState.getGlobalState("showNavigation");
  *
  *    scope.$on("appState.globalState.changed", function (e, args) {
  *               if (args.key === "showNavigation") {
  *                   scope.showTree = args.value;
  *               }
  *           });  
  * </pre>
 *
 * ####Subscribe to section-state changes
 *
 * <pre>
 *    scope.currentSection = appState.getSectionState("currentSection");
 *
 *    scope.$on("appState.sectionState.changed", function (e, args) {
 *               if (args.key === "currentSection") {
 *                   scope.currentSection = args.value;
 *               }
 *           });  
 * </pre>
 */
function appState($rootScope) {
    
    //Define all variables here - we are never returning this objects so they cannot be publicly mutable
    // changed, we only expose methods to interact with the values.

    var globalState = {
        showNavigation: null,
        touchDevice: null,
        showTray: null,
        stickyNavigation: null,
        navMode: null
    };
    
    var sectionState = {
        //The currently active section
        currentSection: null,
        showSearchResults: null
    };

    var treeState = {
        //The currently selected node
        selectedNode: null,
        //The currently loaded root node reference - depending on the section loaded this could be a section root or a normal root.
        //We keep this reference so we can lookup nodes to interact with in the UI via the tree service
        currentRootNode: null
    };
    
    var menuState = {
        //this list of menu items to display
        menuActions: null,
        //the title to display in the context menu dialog
        dialogTitle: null,
        //The tree node that the ctx menu is launched for
        currentNode: null,
        //Whether the menu's dialog is being shown or not
        showMenuDialog: null,
        //Whether the context menu is being shown or not
        showMenu: null
    };

    /** function to validate and set the state on a state object */
    function setState(stateObj, key, value, stateObjName) {
        if (!_.has(stateObj, key)) {
            throw "The variable " + key + " does not exist in " + stateObjName;
        }
        var changed = stateObj[key] !== value;
        stateObj[key] = value;
        if (changed) {
            $rootScope.$broadcast("appState." + stateObjName + ".changed", { key: key, value: value });
        }
    }
    
    /** function to validate and set the state on a state object */
    function getState(stateObj, key, stateObjName) {
        if (!_.has(stateObj, key)) {
            throw "The variable " + key + " does not exist in " + stateObjName;
        }
        return stateObj[key];
    }

    return {

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#getGlobalState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Returns the current global state value by key - we do not return an object reference here - we do NOT want this
         * to be publicly mutable and allow setting arbitrary values
         *
         */
        getGlobalState: function (key) {
            return getState(globalState, key, "globalState");
        },

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#setGlobalState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Sets a global state value by key
         *
         */
        setGlobalState: function (key, value) {
            setState(globalState, key, value, "globalState");
        },

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#getSectionState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Returns the current section state value by key - we do not return an object here - we do NOT want this
         * to be publicly mutable and allow setting arbitrary values
         *
         */
        getSectionState: function (key) {
            return getState(sectionState, key, "sectionState");            
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#setSectionState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Sets a section state value by key
         *
         */
        setSectionState: function(key, value) {
            setState(sectionState, key, value, "sectionState");
        },

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#getTreeState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Returns the current tree state value by key - we do not return an object here - we do NOT want this
         * to be publicly mutable and allow setting arbitrary values
         *
         */
        getTreeState: function (key) {
            return getState(treeState, key, "treeState");
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#setTreeState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Sets a section state value by key
         *
         */
        setTreeState: function (key, value) {
            setState(treeState, key, value, "treeState");
        },

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#getMenuState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Returns the current menu state value by key - we do not return an object here - we do NOT want this
         * to be publicly mutable and allow setting arbitrary values
         *
         */
        getMenuState: function (key) {
            return getState(menuState, key, "menuState");
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#setMenuState
         * @methodOf umbraco.services.appState
         * @function
         *
         * @description
         * Sets a section state value by key
         *
         */
        setMenuState: function (key, value) {
            setState(menuState, key, value, "menuState");
        },

    };
}
angular.module('umbraco.services').factory('appState', appState);

/**
 * @ngdoc service
 * @name umbraco.services.editorState
 * @function
 *
 * @description
 * Tracks the parent object for complex editors by exposing it as 
 * an object reference via editorState.current.entity
 *
 * it is possible to modify this object, so should be used with care
 */
angular.module('umbraco.services').factory("editorState", function() {

    var current = null;
    var state = {

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#set
         * @methodOf umbraco.services.editorState
         * @function
         *
         * @description
         * Sets the current entity object for the currently active editor
         * This is only used when implementing an editor with a complex model
         * like the content editor, where the model is modified by several
         * child controllers. 
         */
        set: function (entity) {
            current = entity;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#reset
         * @methodOf umbraco.services.editorState
         * @function
         *
         * @description
         * Since the editorstate entity is read-only, you cannot set it to null
         * only through the reset() method
         */
        reset: function() {
            current = null;
        }

        /**
         * @ngdoc function
         * @name umbraco.services.angularHelper#current
         * @methodOf umbraco.services.editorState
         * @function
         *
         * @description
         * Returns an object reference to the current editor entity.
         * the entity is the root object of the editor.
         * EditorState is used by property/parameter editors that need
         * access to the entire entity being edited, not just the property/parameter 
         *
         * editorState.current can not be overwritten, you should only read values from it
         * since modifying individual properties should be handled by the property editors
         */
    };

    //create a get/set property but don't allow setting
    Object.defineProperty(state, "current", {
        get: function () {
            return current;
        },
        set: function (value) {
            throw "Use editorState.set to set the value of the current entity";
        },
    });

    return state;
});
/**
 * @ngdoc service
 * @name umbraco.services.assetsService
 *
 * @requires $q 
 * @requires angularHelper
 *  
 * @description
 * Promise-based utillity service to lazy-load client-side dependencies inside angular controllers.
 * 
 * ##usage
 * To use, simply inject the assetsService into any controller that needs it, and make
 * sure the umbraco.services module is accesible - which it should be by default.
 *
 * <pre>
 *      angular.module("umbraco").controller("my.controller". function(assetsService){
 *          assetsService.load(["script.js", "styles.css"], $scope).then(function(){
 *                 //this code executes when the dependencies are done loading
 *          });
 *      });
 * </pre> 
 *
 * You can also load individual files, which gives you greater control over what attibutes are passed to the file, as well as timeout
 *
 * <pre>
 *      angular.module("umbraco").controller("my.controller". function(assetsService){
 *          assetsService.loadJs("script.js", $scope, {charset: 'utf-8'}, 10000 }).then(function(){
 *                 //this code executes when the script is done loading
 *          });
 *      });
 * </pre>
 *
 * For these cases, there are 2 individual methods, one for javascript, and one for stylesheets:
 *
 * <pre>
 *      angular.module("umbraco").controller("my.controller". function(assetsService){
 *          assetsService.loadCss("stye.css", $scope, {media: 'print'}, 10000 }).then(function(){
 *                 //loadcss cannot determine when the css is done loading, so this will trigger instantly
 *          });
 *      });
 * </pre>  
 */
angular.module('umbraco.services')
.factory('assetsService', function ($q, $log, angularHelper) {

    return {
        /**
         * @ngdoc method
         * @name umbraco.services.assetsService#loadCss
         * @methodOf umbraco.services.assetsService
         *
         * @description
         * Injects a file as a stylesheet into the document head
         * 
         * @param {String} path path to the css file to load
         * @param {Scope} scope optional scope to pass into the loader
         * @param {Object} keyvalue collection of attributes to pass to the stylesheet element  
         * @param {Number} timeout in milliseconds
         * @returns {Promise} Promise object which resolves when the file has loaded
         */
        loadCss : function(path, scope, attributes, timeout){
            var deferred = $q.defer();
            var t = timeout || 5000;
            var a = attributes || undefined;

            yepnope.injectCss(path, function () {

             if (!scope) {
                 deferred.resolve(true);
             }else{
                 angularHelper.safeApply(scope, function () {
                     deferred.resolve(true);
                 });
             }

            },a,t);

            return deferred.promise;
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.assetsService#loadJs
         * @methodOf umbraco.services.assetsService
         *
         * @description
         * Injects a file as a javascript into the document
         * 
         * @param {String} path path to the js file to load
         * @param {Scope} scope optional scope to pass into the loader
         * @param {Object} keyvalue collection of attributes to pass to the script element  
         * @param {Number} timeout in milliseconds
         * @returns {Promise} Promise object which resolves when the file has loaded
         */
        loadJs : function(path, scope, attributes, timeout){
            var deferred = $q.defer();
            var t = timeout || 5000;
            var a = attributes || undefined;

            yepnope.injectJs(path, function () {

              if (!scope) {
                  deferred.resolve(true);
              }else{
                  angularHelper.safeApply(scope, function () {
                      deferred.resolve(true);
                  });
              }

            },a,t);

            return deferred.promise;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.assetsService#load
         * @methodOf umbraco.services.assetsService
         *
         * @description
         * Injects a collection of files, this can be a mixed collection of css and js files, the loader will determine how to load them
         * 
         * **Warning:** if the collection of files contains a .css file, you will in some cases not receive a resolved promise, it is therefore prefered to use the individual loadCss and loadJs methods
         *
         * @param {Array} pathArray string array of paths to the files to load
         * @param {Scope} scope optional scope to pass into the loader
         * @returns {Promise} Promise object which resolves when all the files has loaded
         */
        load: function (pathArray, scope) {
            var deferred = $q.defer();
           
            var nonEmpty = _.reject(pathArray, function(item) {
                return item === undefined || item === "";
            });

            //don't load anything if there's nothing to load
            if (nonEmpty.length > 0) {

                yepnope({
                    load: pathArray,
                    complete: function() {

                        //if a scope is supplied then we need to make a digest here because
                        // deferred only executes in a digest. This might be required if we 
                        // are doing a load script after an http request or some other async call.
                        if (!scope) {
                            deferred.resolve(true);
                        }
                        else {
                            angularHelper.safeApply(scope, function () {
                                deferred.resolve(true);
                            });
                        }
                    }
                });
            }
            else {
                if (!scope) {
                    deferred.resolve(true);
                }
                else {
                    angularHelper.safeApply(scope, function () {
                        deferred.resolve(true);
                    });
                }
            }

            return deferred.promise;
        }
    };
});

/**
* @ngdoc service
* @name umbraco.services.contentEditingHelper
* @description A helper service for most editors, some methods are specific to content/media/member model types but most are used by 
* all editors to share logic and reduce the amount of replicated code among editors.
**/
function contentEditingHelper($location, $routeParams, notificationsService, serverValidationManager, dialogService, formHelper, appState) {

    return {
        

        /**
         * @ngdoc method
         * @name umbraco.services.contentEditingHelper#getAllProps
         * @methodOf umbraco.services.contentEditingHelper
         * @function
         *
         * @description
         * Returns all propertes contained for the content item (since the normal model has properties contained inside of tabs)
         */
        getAllProps: function (content) {
            var allProps = [];

            for (var i = 0; i < content.tabs.length; i++) {
                for (var p = 0; p < content.tabs[i].properties.length; p++) {
                    allProps.push(content.tabs[i].properties[p]);
                }
            }

            return allProps;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.contentEditingHelper#reBindChangedProperties
         * @methodOf umbraco.services.contentEditingHelper
         * @function
         *
         * @description
         * re-binds all changed property values to the origContent object from the savedContent object and returns an array of changed properties.
         */
        reBindChangedProperties: function (origContent, savedContent) {

            var changed = [];

            //get a list of properties since they are contained in tabs
            var allOrigProps = this.getAllProps(origContent);
            var allNewProps = this.getAllProps(savedContent);

            function getNewProp(alias) {                
                return _.find(allNewProps, function (item) {
                    return item.alias === alias;
                });
            }

            //a method to ignore built-in prop changes
            var shouldIgnore = function(propName) {
                return _.some(["tabs", "notifications", "ModelState", "tabs", "properties"], function(i) {
                    return i === propName;
                });
            };
            //check for changed built-in properties of the content
            for (var o in origContent) {
                
                //ignore the ones listed in the array
                if (shouldIgnore(o)) {
                    continue;
                }
                
                if (!_.isEqual(origContent[o], savedContent[o])) {
                    origContent[o] = savedContent[o];
                }
            }

            //check for changed properties of the content
            for (var p in allOrigProps) {
                var newProp = getNewProp(allOrigProps[p].alias);
                if (newProp && !_.isEqual(allOrigProps[p].value, newProp.value)) {

                    //they have changed so set the origContent prop to the new one
                    var origVal = allOrigProps[p].value;
                    allOrigProps[p].value = newProp.value;
                    
                    //instead of having a property editor $watch their expression to check if it has 
                    // been updated, instead we'll check for the existence of a special method on their model
                    // and just call it.
                    if (angular.isFunction(allOrigProps[p].onValueChanged)) {
                        //send the newVal + oldVal
                        allOrigProps[p].onValueChanged(allOrigProps[p].value, origVal);
                    }

                    changed.push(allOrigProps[p]);
                }
            }

            return changed;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.contentEditingHelper#handleSaveError
         * @methodOf umbraco.services.contentEditingHelper
         * @function
         *
         * @description
         * A function to handle what happens when we have validation issues from the server side
         */
        handleSaveError: function (args) {
             
            if (!args.err) {
                throw "args.err cannot be null";
            }
            if (args.redirectOnFailure === undefined || args.redirectOnFailure === null) {
                throw "args.redirectOnFailure must be set to true or false";
            }

            //When the status is a 400 status with a custom header: X-Status-Reason: Validation failed, we have validation errors.
            //Otherwise the error is probably due to invalid data (i.e. someone mucking around with the ids or something).
            //Or, some strange server error
            if (args.err.status === 400) {
                //now we need to look through all the validation errors
                if (args.err.data && (args.err.data.ModelState)) {
                    
                    //wire up the server validation errs
                    formHelper.handleServerValidation(args.err.data.ModelState);

                    if (!args.redirectOnFailure || !this.redirectToCreatedContent(args.err.data.id, args.err.data.ModelState)) {
                        //we are not redirecting because this is not new content, it is existing content. In this case
                        // we need to detect what properties have changed and re-bind them with the server data. Then we need
                        // to re-bind any server validation errors after the digest takes place.

                        if (args.rebindCallback && angular.isFunction(args.rebindCallback)) {
                            args.rebindCallback();
                        }
                        
                        serverValidationManager.executeAndClearAllSubscriptions();
                    }

                    //indicates we've handled the server result
                    return true;
                }
                else {
                    dialogService.ysodDialog(args.err);
                }
            }
            else {
                dialogService.ysodDialog(args.err);
            }

            return false;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.contentEditingHelper#handleSuccessfulSave
         * @methodOf umbraco.services.contentEditingHelper
         * @function
         *
         * @description
         * A function to handle when saving a content item is successful. This will rebind the values of the model that have changed
         * ensure the notifications are displayed and that the appropriate events are fired. This will also check if we need to redirect
         * when we're creating new content.
         */
        handleSuccessfulSave: function (args) {

            if (!args) {
                throw "args cannot be null";
            }
            if (!args.savedContent) {
                throw "args.savedContent cannot be null";
            }

            if (!this.redirectToCreatedContent(args.redirectId ? args.redirectId : args.savedContent.id)) {
                
                //we are not redirecting because this is not new content, it is existing content. In this case
                // we need to detect what properties have changed and re-bind them with the server data.
                //call the callback
                if (args.rebindCallback && angular.isFunction(args.rebindCallback)) {
                    args.rebindCallback();
                }
            }
        },

        /**
         * @ngdoc function
         * @name umbraco.services.contentEditingHelper#redirectToCreatedContent
         * @methodOf umbraco.services.contentEditingHelper
         * @function
         *
         * @description
         * Changes the location to be editing the newly created content after create was successful.
         * We need to decide if we need to redirect to edito mode or if we will remain in create mode. 
         * We will only need to maintain create mode if we have not fulfilled the basic requirements for creating an entity which is at least having a name.
         */
        redirectToCreatedContent: function (id, modelState) {

            //only continue if we are currently in create mode and if there is no 'Name' modelstate errors
            // since we need at least a name to create content.
            if ($routeParams.create && (!modelState || !modelState["Name"])) {

                //need to change the location to not be in 'create' mode. Currently the route will be something like:
                // /belle/#/content/edit/1234?doctype=newsArticle&create=true
                // but we need to remove everything after the query so that it is just:
                // /belle/#/content/edit/9876 (where 9876 is the new id)

                //clear the query strings
                $location.search("");
                
                //change to new path
                $location.path("/" + $routeParams.section + "/" + $routeParams.tree  + "/" + $routeParams.method + "/" + id);
                //don't add a browser history for this
                $location.replace();
                return true;
            }
            return false;
        }
    };
}
angular.module('umbraco.services').factory('contentEditingHelper', contentEditingHelper);
/**
 * @ngdoc service
 * @name umbraco.services.dialogService
 *
 * @requires $rootScope 
 * @requires $compile
 * @requires $http
 * @requires $log
 * @requires $q
 * @requires $templateCache
 *  
 * @description
 * Application-wide service for handling modals, overlays and dialogs
 * By default it injects the passed template url into a div to body of the document
 * And renders it, but does also support rendering items in an iframe, incase
 * serverside processing is needed, or its a non-angular page
 *
 * ##usage
 * To use, simply inject the dialogService into any controller that needs it, and make
 * sure the umbraco.services module is accesible - which it should be by default.
 *
 * <pre>
 *    var dialog = dialogService.open({template: 'path/to/page.html', show: true, callback: done});
 *    functon done(data){
 *      //The dialog has been submitted 
 *      //data contains whatever the dialog has selected / attached
 *    }     
 * </pre> 
 */

angular.module('umbraco.services')
.factory('dialogService', function ($rootScope, $compile, $http, $timeout, $q, $templateCache, appState) {

       var dialogs = [];
       
       /** Internal method that removes all dialogs */
       function removeAllDialogs(args) {
           for (var i = 0; i < dialogs.length; i++) {
               var dialog = dialogs[i];
               dialog.close(args);
               dialogs.splice(i, 1);
           }
       }

       /** Internal method that handles opening all dialogs */
       function openDialog(options) {
           var defaults = {
              container: $("body"),
              animation: "fade",
              modalClass: "umb-modal",
              width: "100%",
              inline: false,
              iframe: false,
              show: true,
              template: "views/common/notfound.html",
              callback: undefined,
              closeCallback: undefined,
              element: undefined,
              //this allows us to pass in data to the dialog if required which can be used to configure the dialog
              //and re-use it for different purposes. It will set on to the $scope.dialogData if it is defined.
              dialogData: undefined
           };
           
           var dialog = angular.extend(defaults, options);
           var scope = options.scope || $rootScope.$new();
           
           //Modal dom obj and unique id
           dialog.element = $('<div ng-swipe-right="swipeHide($event)"  data-backdrop="false"></div>');
           var id = dialog.template.replace('.html', '').replace('.aspx', '').replace(/[\/|\.|:\&\?\=]/g, "-") + '-' + scope.$id;

           if (options.inline) {
               dialog.animation = "";
           }
           else {
               dialog.element.addClass("modal");
               dialog.element.addClass("hide");
           }

           //set the id and add classes
           dialog.element
               .attr('id', id)
               .addClass(dialog.animation)
               .addClass(dialog.modalClass);

           //push the modal into the global modal collection
           //we halt the .push because a link click will trigger a closeAll right away
           $timeout(function () {
               dialogs.push(dialog);
           }, 500);
           

           dialog.close = function(data) {
              if (dialog.closeCallback) {
                   dialog.closeCallback(data);
              }

              if(dialog.element){
                 dialog.element.modal('hide');

                 //this is not entirely enough since the damn
                 //webforms scriploader still complains
                 if(dialog.iframe){
                    dialog.element.find("iframe").attr("src", "about:blank");
                    $timeout(function(){
                      dialog.element.remove();
                    }, 1000); 
                 }else{
                    dialog.element.remove();
                 }
               }
           };

           //if iframe is enabled, inject that instead of a template
           if (dialog.iframe) {
               var html = $("<iframe src='" + dialog.template + "' class='auto-expand' style='border: none; width: 100%; height: 100%;'></iframe>");
               dialog.element.html(html);
  
               //append to body or whatever element is passed in as options.containerElement
               dialog.container.append(dialog.element);

               // Compile modal content
               $timeout(function () {
                   $compile(dialog.element)(dialog.scope);
               });

               dialog.element.css("width", dialog.width);

               //Autoshow 
               if (dialog.show) {
                   dialog.element.modal('show');
               }

               dialog.scope = scope;
               return dialog;
           }
           else {
               
             //We need to load the template with an httpget and once it's loaded we'll compile and assign the result to the container
             // object. However since the result could be a promise or just data we need to use a $q.when. We still need to return the 
             // $modal object so we'll actually return the modal object synchronously without waiting for the promise. Otherwise this openDialog
             // method will always need to return a promise which gets nasty because of promises in promises plus the result just needs a reference
             // to the $modal object which will not change (only it's contents will change).
             $q.when($templateCache.get(dialog.template) || $http.get(dialog.template, { cache: true }).then(function(res) { return res.data; }))
                 .then(function onSuccess(template) {

                     // Build modal object
                     dialog.element.html(template);

                     //append to body or other container element  
                     dialog.container.append(dialog.element);
                     
                     // Compile modal content
                     $timeout(function() {
                         $compile(dialog.element)(scope);
                     });

                     scope.dialogOptions = dialog;
                     
                     //Scope to handle data from the modal form
                     scope.dialogData = dialog.dialogData ? dialog.dialogData : {};
                     scope.dialogData.selection = [];

                     // Provide scope display functions
                     //this passes the modal to the current scope
                     scope.$modal = function(name) {
                         dialog.element.modal(name);
                     };

                     scope.swipeHide = function(e){
                       if(appState.getGlobalState("touchDevice")){
                            var selection = window.getSelection();
                            if(selection.type !== "Range"){
                              scope.hide();  
                            }
                        }
                     };

                     scope.hide = function() {
                         dialog.element.modal('hide');

                         dialog.element.remove();
                         $("#" + dialog.element.attr("id")).remove();
                     };

                     //basic events for submitting and closing
                     scope.submit = function(data) {
                         if (dialog.callback) {
                             dialog.callback(data);
                         }

                         dialog.element.modal('hide');
                         dialog.element.remove();
                         $("#" + dialog.element.attr("id")).remove();
                     };

                     scope.close = function(data) {
                        dialog.close(data);
                     };

                     scope.show = function() {
                         dialog.element.modal('show');
                     };

                     scope.select = function(item) {
                        var i = scope.dialogData.selection.indexOf(item);
                         if (i < 0) {
                             scope.dialogData.selection.push(item);
                         }else{
                            scope.dialogData.selection.splice(i, 1);
                         }
                     };

                     scope.dismiss = scope.hide;

                     // Emit modal events
                     angular.forEach(['show', 'shown', 'hide', 'hidden'], function(name) {
                         dialog.element.on(name, function(ev) {
                             scope.$emit('modal-' + name, ev);
                         });
                     });

                     // Support autofocus attribute
                     dialog.element.on('shown', function(event) {
                         $('input[autofocus]', dialog.element).first().trigger('focus');
                     });

                     //Autoshow 
                     if (dialog.show) {
                         dialog.element.modal('show');
                     }

                     dialog.scope = scope;
               });
               
               //Return the modal object outside of the promise!
               return dialog;
           }
       }

       /** Handles the closeDialogs event */
       $rootScope.$on("closeDialogs", function (evt, args) {
           removeAllDialogs(args);
       });

       return {
           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#open
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a modal rendering a given template url.
            *
            * @param {Object} options rendering options
            * @param {DomElement} options.container the DOM element to inject the modal into, by default set to body
            * @param {Function} options.callback function called when the modal is submitted
            * @param {String} options.template the url of the template
            * @param {String} options.animation animation csss class, by default set to "fade"
            * @param {String} options.modalClass modal css class, by default "umb-modal"
            * @param {Bool} options.show show the modal instantly
            * @param {Object} options.scope scope to attach the modal to, by default rootScope.new()
            * @param {Bool} options.iframe load template in an iframe, only needed for serverside templates
            * @param {Int} options.width set a width on the modal, only needed for iframes
            * @param {Bool} options.inline strips the modal from any animation and wrappers, used when you want to inject a dialog into an existing container
            * @returns {Object} modal object
            */
           open: function (options) {               
               return openDialog(options);
           },
           
           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#close
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Closes a specific dialog
            * @param {Object} dialog the dialog object to close
            * @param {Object} args if specified this object will be sent to any callbacks registered on the dialogs.
            */
           close: function (dialog, args) {
              if(dialog){
                  dialog.close(args);
              }              
           },
           
           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#closeAll
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Closes all dialogs
            * @param {Object} args if specified this object will be sent to any callbacks registered on the dialogs.
            */
           closeAll: function(args) {
               removeAllDialogs(args);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#mediaPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a media picker in a modal, the callback returns an array of selected media items
            * @param {Object} options mediapicker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           mediaPicker: function (options) {
            options.template = 'views/common/dialogs/mediaPicker.html';
            options.show = true;
            return openDialog(options);
           },


           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#contentPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a content picker tree in a modal, the callback returns an array of selected documents
            * @param {Object} options content picker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {$scope} options.multipicker should the picker return one or multiple items
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           contentPicker: function (options) {
               options.template = 'views/common/dialogs/contentPicker.html';
               options.show = true;
              return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#linkPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a link picker tree in a modal, the callback returns a single link
            * @param {Object} options content picker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           linkPicker: function (options) {
               options.template = 'views/common/dialogs/linkPicker.html';
               options.show = true;
              return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#macroPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a mcaro picker in a modal, the callback returns a object representing the macro and it's parameters
            * @param {Object} options macropicker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           macroPicker: function (options) {
                options.template = 'views/common/dialogs/insertmacro.html';
                options.show = true;
                options.modalClass = "span7 umb-modal";
                return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#memberPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a member picker in a modal, the callback returns a object representing the selected member
            * @param {Object} options member picker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {$scope} options.multiPicker should the tree pick one or multiple members before returning
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           memberPicker: function (options) {
               options.template = 'views/common/dialogs/memberPicker.html';
               options.show = true;
              return openDialog(options);
           },
           
           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#memberGroupPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a member group picker in a modal, the callback returns a object representing the selected member
            * @param {Object} options member group picker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {$scope} options.multiPicker should the tree pick one or multiple members before returning
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           memberGroupPicker: function (options) {
               options.template = 'views/common/dialogs/memberGroupPicker.html';
               options.show = true;
               return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#iconPicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a icon picker in a modal, the callback returns a object representing the selected icon
            * @param {Object} options iconpicker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           iconPicker: function (options) {
                options.template = 'views/common/dialogs/iconPicker.html';
                options.show = true;
                return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#treePicker
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a tree picker in a modal, the callback returns a object representing the selected tree item
            * @param {Object} options iconpicker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {$scope} options.section tree section to display
            * @param {$scope} options.treeAlias specific tree to display
            * @param {$scope} options.multiPicker should the tree pick one or multiple items before returning
            * @param {Function} options.callback callback function
            * @returns {Object} modal object
            */
           treePicker: function (options) {
                options.template = 'views/common/dialogs/treePicker.html';
                options.show = true;
                return openDialog(options);
           },

           /**
            * @ngdoc method
            * @name umbraco.services.dialogService#propertyDialog
            * @methodOf umbraco.services.dialogService
            *
            * @description
            * Opens a dialog with a chosen property editor in, a value can be passed to the modal, and this value is returned in the callback
            * @param {Object} options mediapicker dialog options object
            * @param {$scope} options.scope dialog scope
            * @param {Function} options.callback callback function
            * @param {String} editor editor to use to edit a given value and return on callback
            * @param {Object} value value sent to the property editor
            * @returns {Object} modal object
            */
          propertyDialog: function (options) {
              options.template = 'views/common/dialogs/property.html';
              options.show = true;
              return openDialog(options);
          },
           
           /**
           * @ngdoc method
           * @name umbraco.services.dialogService#ysodDialog
           * @methodOf umbraco.services.dialogService
           *
           * @description
           * Opens a dialog to an embed dialog 
           */
          embedDialog: function (options) {
              options.template = 'views/common/dialogs/rteembed.html';
              options.show = true;
              return openDialog(options);
          },
           /**
           * @ngdoc method
           * @name umbraco.services.dialogService#ysodDialog
           * @methodOf umbraco.services.dialogService
           *
           * @description
           * Opens a dialog to show a custom YSOD
           */
           ysodDialog: function (ysodError) {

               var newScope = $rootScope.$new();
               newScope.error = ysodError;
               return openDialog({
                   modalClass: "umb-modal wide",
                   scope: newScope,
                   //callback: options.callback,
                   template: 'views/common/dialogs/ysod.html',
                   show: true
               });
           }
       };
   });
/** Used to broadcast and listen for global events and allow the ability to add async listeners to the callbacks */
function eventsService($q, $rootScope) {
	
    return {
        
        /** raise an event with a given name, returns an array of promises for each listener */
        publish: function (name, args) {            

            //there are no listeners
            if (!$rootScope.$$listeners[name]) {
                return [];
            }

            //setup a deferred promise for each listener
            var deferred = [];
            for (var i = 0; i < $rootScope.$$listeners[name].length; i++) {
                deferred.push($q.defer());
            }
            //create a new event args object to pass to the 
            // $broadcast containing methods that will allow listeners
            // to return data in an async if required
            var eventArgs = {
                args: args,
                reject: function (a) {
                    deferred.pop().reject(a);
                },
                resolve: function (a) {
                    deferred.pop().resolve(a);
                }
            };
            
            //send the event
            $rootScope.$broadcast(name, eventArgs);
            
            //return an array of promises
            var promises = _.map(deferred, function(p) {
                return p.promise;
            });
            return promises;
        },

        /** subscribe to a method, or use scope.$on = same thing */
		subscribe: function(name, callback) {
		    return $rootScope.$on(name, callback);
		},
		
        /** pass in the result of subscribe to this method, or just call the method returned from subscribe to unsubscribe */
		unsubscribe: function(handle) {
		    if (angular.isFunction(handle)) {
		        handle();
		    }		    
		}

	};
}

angular.module('umbraco.services').factory('eventsService', eventsService);
/**
 * @ngdoc service
 * @name umbraco.services.fileManager
 * @function
 *
 * @description
 * Used by editors to manage any files that require uploading with the posted data, normally called by property editors
 * that need to attach files.
 * When a route changes successfully, we ensure that the collection is cleared.
 */
function fileManager($rootScope) {

    var fileCollection = [];

    //Whenever a route changes - clear the curent file collection, the file collection is only relavent
    // when working in an editor and submitting data to the server.
    //This ensures that memory remains clear of any files and that the editors don't have to manually clear the files.
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        fileCollection = [];
    });

    return {
        /**
         * @ngdoc function
         * @name umbraco.services.fileManager#addFiles
         * @methodOf umbraco.services.fileManager
         * @function
         *
         * @description
         *  Attaches files to the current manager for the current editor for a particular property, if an empty array is set
         *   for the files collection that effectively clears the files for the specified editor.
         */
        setFiles: function(propertyId, files) {
            //this will clear the files for the current property and then add the new ones for the current property
            fileCollection = _.reject(fileCollection, function (item) {
                return item.id === propertyId;
            });
            for (var i = 0; i < files.length; i++) {
                //save the file object to the files collection
                fileCollection.push({ id: propertyId, file: files[i] });
            }
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.fileManager#getFiles
         * @methodOf umbraco.services.fileManager
         * @function
         *
         * @description
         *  Returns all of the files attached to the file manager
         */
        getFiles: function() {
            return fileCollection;
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.fileManager#clearFiles
         * @methodOf umbraco.services.fileManager
         * @function
         *
         * @description
         *  Removes all files from the manager
         */
        clearFiles: function () {
            fileCollection = [];
        }
};
}

angular.module('umbraco.services').factory('fileManager', fileManager);
/**
 * @ngdoc service
 * @name umbraco.services.formHelper
 * @function
 *
 * @description
 * A utility class used to streamline how forms are developed, to ensure that validation is check and displayed consistently and to ensure that the correct events
 * fire when they need to.
 */
function formHelper(angularHelper, serverValidationManager, $timeout, notificationsService, dialogService) {
    return {

        /**
         * @ngdoc function
         * @name umbraco.services.formHelper#submitForm
         * @methodOf umbraco.services.formHelper
         * @function
         *
         * @description
         * Called by controllers when submitting a form - this ensures that all client validation is checked, 
         * server validation is cleared, that the correct events execute and status messages are displayed.
         * This returns true if the form is valid, otherwise false if form submission cannot continue.
         * 
         * @param {object} args An object containing arguments for form submission
         */
        submitForm: function (args) {

            var currentForm;

            if (!args) {
                throw "args cannot be null";
            }
            if (!args.scope) {
                throw "args.scope cannot be null";
            }
            if (!args.formCtrl) {
                //try to get the closest form controller
                currentForm = angularHelper.getRequiredCurrentForm(args.scope);
            }
            else {
                currentForm = args.formCtrl;
            }
            //if no statusPropertyName is set we'll default to formStatus.
            if (!args.statusPropertyName) {
                args.statusPropertyName = "formStatus";
            }
            //if no statusTimeout is set, we'll  default to 2500 ms
            if (!args.statusTimeout) {
                args.statusTimeout = 2500;
            }
            
            //the first thing any form must do is broadcast the formSubmitting event
            args.scope.$broadcast("formSubmitting", { scope: args.scope });

            //then check if the form is valid
            if (!args.skipValidation) {                
                if (currentForm.$invalid) {
                    return false;
                }
            }

            //reset the server validations
            serverValidationManager.reset();
            
            //check if a form status should be set on the scope
            if (args.statusMessage) {
                args.scope[args.statusPropertyName] = args.statusMessage;

                //clear the message after the timeout
                $timeout(function () {
                    args.scope[args.statusPropertyName] = undefined;
                }, args.statusTimeout);
            }

            return true;
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.formHelper#submitForm
         * @methodOf umbraco.services.formHelper
         * @function
         *
         * @description
         * Called by controllers when a form has been successfully submitted. the correct events execute 
         * and that the notifications are displayed if there are any.
         * 
         * @param {object} args An object containing arguments for form submission
         */
        resetForm: function (args) {
            if (!args) {
                throw "args cannot be null";
            }
            if (!args.scope) {
                throw "args.scope cannot be null";
            }
            
            //if no statusPropertyName is set we'll default to formStatus.
            if (!args.statusPropertyName) {
                args.statusPropertyName = "formStatus";
            }
            //clear the status
            args.scope[args.statusPropertyName] = null;

            if (angular.isArray(args.notifications)) {
                for (var i = 0; i < args.notifications.length; i++) {
                    notificationsService.showNotification(args.notifications[i]);
                }
            }

            args.scope.$broadcast("formSubmitted", { scope: args.scope });
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.formHelper#handleError
         * @methodOf umbraco.services.formHelper
         * @function
         *
         * @description
         * Needs to be called when a form submission fails, this will wire up all server validation errors in ModelState and
         * add the correct messages to the notifications. If a server error has occurred this will show a ysod.
         * 
         * @param {object} err The error object returned from the http promise
         */
        handleError: function (err) {            
            //When the status is a 400 status with a custom header: X-Status-Reason: Validation failed, we have validation errors.
            //Otherwise the error is probably due to invalid data (i.e. someone mucking around with the ids or something).
            //Or, some strange server error
            if (err.status === 400) {
                //now we need to look through all the validation errors
                if (err.data && (err.data.ModelState)) {

                    //wire up the server validation errs
                    this.handleServerValidation(err.data.ModelState);

                    //execute all server validation events and subscribers
                    serverValidationManager.executeAndClearAllSubscriptions();                    
                }
                else {
                    dialogService.ysodDialog(err);
                }
            }
            else {
                dialogService.ysodDialog(err);
            }
        },

        /**
         * @ngdoc function
         * @name umbraco.services.formHelper#handleServerValidation
         * @methodOf umbraco.services.formHelper
         * @function
         *
         * @description
         * This wires up all of the server validation model state so that valServer and valServerField directives work
         * 
         * @param {object} err The error object returned from the http promise
         */
        handleServerValidation: function(modelState) {
            for (var e in modelState) {

                //the alias in model state can be in dot notation which indicates
                // * the first part is the content property alias
                // * the second part is the field to which the valiation msg is associated with
                //There will always be at least 2 parts for properties since all model errors for properties are prefixed with "Properties"
                //If it is not prefixed with "Properties" that means the error is for a field of the object directly.

                var parts = e.split(".");
                if (parts.length > 1) {
                    var propertyAlias = parts[1];

                    //if it contains 2 '.' then we will wire it up to a property's field
                    if (parts.length > 2) {
                        //add an error with a reference to the field for which the validation belongs too
                        serverValidationManager.addPropertyError(propertyAlias, parts[2], modelState[e][0]);
                    }
                    else {
                        //add a generic error for the property, no reference to a specific field
                        serverValidationManager.addPropertyError(propertyAlias, "", modelState[e][0]);
                    }

                }
                else {
                    //the parts are only 1, this means its not a property but a native content property
                    serverValidationManager.addFieldError(parts[0], modelState[e][0]);
                }

                //add to notifications
                notificationsService.error("Validation", modelState[e][0]);
            }
        }
    };
}
angular.module('umbraco.services').factory('formHelper', formHelper);
angular.module('umbraco.services')
	.factory('helpService', function ($http, $q){
		var helpTopics = {};

		var defaultUrl = "http://our.umbraco.org/rss/help";
		var tvUrl = "http://umbraco.tv/feeds/help";

		function getCachedHelp(url){
			if(helpTopics[url]){
				return helpTopics[cacheKey];
			}else{
				return null;
			}
		}

		function setCachedHelp(url, data){
			helpTopics[url] = data;
		}

		function fetchUrl(url){
			var deferred = $q.defer();
			var found = getCachedHelp(url);

			if(found){
				deferred.resolve(found);
			}else{

				var proxyUrl = "dashboard/feedproxy.aspx?url=" + url; 
				$http.get(proxyUrl).then(function(data){
					var feed = $(data.data);
					var topics = [];

					$('item', feed).each(function (i, item) {
						var topic = {};
						topic.thumbnail = $(item).find('thumbnail').attr('url');
						topic.title = $("title", item).text();
						topic.link = $("guid", item).text();
						topic.description = $("description", item).text();
						topics.push(topic);
					});

					setCachedHelp(topics);
					deferred.resolve(topics);
				});
			}

			return deferred.promise;
		}



		var service = {
			findHelp: function (args) {
				var url = service.getUrl(defaultUrl, args);
				return fetchUrl(url);
			},

			findVideos: function (args) {
				var url = service.getUrl(tvUrl, args);
				return fetchUrl(url);
			},

			getUrl: function(url, args){
				return url + "?" + $.param(args);
			}
		};

		return service;

	});
/**
 * @ngdoc service
 * @name umbraco.services.historyService
 *
 * @requires $rootScope 
 * @requires $timeout
 * @requires angularHelper
 *	
 * @description
 * Service to handle the main application navigation history. Responsible for keeping track
 * of where a user navigates to, stores an icon, url and name in a collection, to make it easy
 * for the user to go back to a previous editor / action
 *
 * **Note:** only works with new angular-based editors, not legacy ones
 *
 * ##usage
 * To use, simply inject the historyService into any controller that needs it, and make
 * sure the umbraco.services module is accesible - which it should be by default.
 *
 * <pre>
 *      angular.module("umbraco").controller("my.controller". function(historyService){
 *         historyService.add({
 *								icon: "icon-class",
 *								name: "Editing 'articles',
 *								link: "/content/edit/1234"}
 *							);
 *      }); 
 * </pre> 
 */
angular.module('umbraco.services')
.factory('historyService', function ($rootScope, $timeout, angularHelper) {

	var nArray = [];

	function add(item) {

		var any = _.where(nArray, {link: item.link});

		if(any.length === 0){
			nArray.splice(0,0,item);
			return nArray[0];
		}
	}

	return {
		/**
		 * @ngdoc method
		 * @name umbraco.services.historyService#add
		 * @methodOf umbraco.services.historyService
		 *
		 * @description
		 * Adds a given history item to the users history collection.
		 *
		 * @param {Object} item the history item
		 * @param {String} item.icon icon css class for the list, ex: "icon-image", "icon-doc"
		 * @param {String} item.link route to the editor, ex: "/content/edit/1234"
		 * @param {String} item.name friendly name for the history listing
		 * @returns {Object} history item object
		 */
		add: function (item) {
			var icon = item.icon || "icon-file";
			angularHelper.safeApply($rootScope, function () {
				return add({name: item.name, icon: icon, link: item.link, time: new Date() });
			});
		},
		/**
		 * @ngdoc method
		 * @name umbraco.services.historyService#remove
		 * @methodOf umbraco.services.historyService
		 *
		 * @description
		 * Removes a history item from the users history collection, given an index to remove from.
		 *
		 * @param {Int} index index to remove item from
		 */
		remove: function (index) {
			angularHelper.safeApply($rootScope, function() {
				nArray.splice(index, 1);
			});
		},

		/**
		 * @ngdoc method
		 * @name umbraco.services.historyService#removeAll
		 * @methodOf umbraco.services.historyService
		 *
		 * @description
		 * Removes all history items from the users history collection
		 */
		removeAll: function () {
			angularHelper.safeApply($rootScope, function() {
				nArray = [];
			});
		},

		/**
		 * @ngdoc property
		 * @name umbraco.services.historyService#current
		 * @propertyOf umbraco.services.historyService
		 *
		 * @description
		 * 
		 * @returns {Array} Array of history entries for the current user, newest items first
		 */
		current: nArray,

		/**
		 * @ngdoc method
		 * @name umbraco.services.historyService#getCurrent
		 * @methodOf umbraco.services.historyService
		 *
		 * @description
		 * Method to return the current history collection.
		 *
		 */
		getCurrent: function(){
			return nArray;
		}
	};
});
// This service was based on OpenJS library available in BSD License
// http://www.openjs.com/scripts/events/keyboard_shortcuts/index.php
angular.module('umbraco.services')
.factory('keyboardService', ['$window', '$timeout', function ($window, $timeout) {
	var keyboardManagerService = {};
	var defaultOpt = {
		'type':             'keydown',
		'propagate':        false,
		'inputDisabled':    false,
		'target':           $window.document,
		'keyCode':          false
	};
	
	var isMac = navigator.platform.toUpperCase().indexOf('MAC')>=0;

	// Store all keyboard combination shortcuts
	keyboardManagerService.keyboardEvent = {};


	// Add a new keyboard combination shortcut
	keyboardManagerService.bind = function (label, callback, opt) {

		//replace ctrl key with meta key
		if(isMac){
		  label = label.replace("ctrl","meta");
		}

		//always try to unbind first, so we dont have multiple actions on the same key
		keyboardManagerService.unbind(label);

		var fct, elt, code, k;
		// Initialize opt object
		opt   = angular.extend({}, defaultOpt, opt);
		label = label.toLowerCase();
		elt   = opt.target;
		if(typeof opt.target === 'string'){
			elt = document.getElementById(opt.target);	
		} 

	
		fct = function (e) {
			e = e || $window.event;

			// Disable event handler when focus input and textarea
			if (opt['inputDisabled']) {
				var elt;
				if (e.target){
					elt = e.target;	
				}else if (e.srcElement){
					elt = e.srcElement;	
				} 

				if (elt.nodeType === 3){elt = elt.parentNode;} 
				if (elt.tagName === 'INPUT' || elt.tagName === 'TEXTAREA'){return;}
			}

			// Find out which key is pressed
			if (e.keyCode){
				code = e.keyCode;	
			}else if (e.which){
				code = e.which;	
			} 

			var character = String.fromCharCode(code).toLowerCase();

			if (code === 188){character = ",";} // If the user presses , when the type is onkeydown
			if (code === 190){character = ".";} // If the user presses , when the type is onkeydown

			var keys = label.split("+");
			// Key Pressed - counts the number of valid keypresses - if it is same as the number of keys, the shortcut function is invoked
			var kp = 0;
			// Work around for stupid Shift key bug created by using lowercase - as a result the shift+num combination was broken
			var shift_nums = {
				"`":"~",
				"1":"!",
				"2":"@",
				"3":"#",
				"4":"$",
				"5":"%",
				"6":"^",
				"7":"&",
				"8":"*",
				"9":"(",
				"0":")",
				"-":"_",
				"=":"+",
				";":":",
				"'":"\"",
				",":"<",
				".":">",
				"/":"?",
				"\\":"|"
			};
			// Special Keys - and their codes
			var special_keys = {
				'esc':27,
				'escape':27,
				'tab':9,				
				'space':32,
				'return':13,
				'enter':13,
				'backspace':8,

				'scrolllock':145,
				'scroll_lock':145,
				'scroll':145,
				'capslock':20,
				'caps_lock':20,
				'caps':20,
				'numlock':144,
				'num_lock':144,
				'num':144,

				'pause':19,
				'break':19,

				'insert':45,
				'home':36,
				'delete':46,
				'end':35,

				'pageup':33,
				'page_up':33,
				'pu':33,

				'pagedown':34,
				'page_down':34,
				'pd':34,

				'left':37,
				'up':38,
				'right':39,
				'down':40,

				'f1':112,
				'f2':113,
				'f3':114,
				'f4':115,
				'f5':116,
				'f6':117,
				'f7':118,
				'f8':119,
				'f9':120,
				'f10':121,
				'f11':122,
				'f12':123
			};
			// Some modifiers key
			var modifiers = {
				shift: {
					wanted:		false, 
					pressed:	e.shiftKey ? true : false
				},
				ctrl : {
					wanted:		false, 
					pressed:	e.ctrlKey ? true : false
				},
				alt  : {
					wanted:		false,
					pressed:	e.altKey ? true : false
				},
				meta : { //Meta is Mac specific
					wanted:		false, 
					pressed:	e.metaKey ? true : false
				}
			};
			// Foreach keys in label (split on +)
			var l = keys.length;
			for (var i = 0; i < l; i++) {

				var k=keys[i];
				switch (k) {
					case 'ctrl':
					case 'control':
						kp++;
						modifiers.ctrl.wanted = true;
						break;
					case 'shift':
					case 'alt':
					case 'meta':
						kp++;
						modifiers[k].wanted = true;
						break;
				}

				if (k.length > 1) { // If it is a special key
					if(special_keys[k] === code){
						kp++;
					}

				} else if (opt['keyCode']) { // If a specific key is set into the config
					if (opt['keyCode'] === code) {
						kp++;
					}

				} else { // The special keys did not match
					if(character === k) {
						kp++;
					}else {
						if(shift_nums[character] && e.shiftKey) { // Stupid Shift key bug created by using lowercase
							character = shift_nums[character];
							if(character === k){
								kp++;
							}
						}
					}
				}

			} //for end

			if(kp === keys.length &&
				modifiers.ctrl.pressed === modifiers.ctrl.wanted &&
				modifiers.shift.pressed === modifiers.shift.wanted &&
				modifiers.alt.pressed === modifiers.alt.wanted &&
				modifiers.meta.pressed === modifiers.meta.wanted) {
		        $timeout(function() {
					callback(e);
		        }, 1);

				if(!opt['propagate']) { // Stop the event
					// e.cancelBubble is supported by IE - this will kill the bubbling process.
					e.cancelBubble = true;
					e.returnValue = false;

					// e.stopPropagation works in Firefox.
					if (e.stopPropagation) {
						e.stopPropagation();
						e.preventDefault();
					}
					return false;
				}
			}
		};
		// Store shortcut
		keyboardManagerService.keyboardEvent[label] = {
			'callback': fct,
			'target':   elt,
			'event':    opt['type']
		};

		//Attach the function with the event
		if(elt.addEventListener){
			elt.addEventListener(opt['type'], fct, false);
		}else if(elt.attachEvent){
			elt.attachEvent('on' + opt['type'], fct);
		}else{
			elt['on' + opt['type']] = fct;
		}
	};
	// Remove the shortcut - just specify the shortcut and I will remove the binding
	keyboardManagerService.unbind = function (label) {
		label = label.toLowerCase();
		var binding = keyboardManagerService.keyboardEvent[label];
		delete(keyboardManagerService.keyboardEvent[label]);
		
		if(!binding){return;}

		var type		= binding['event'],
		elt			= binding['target'],
		callback	= binding['callback'];
		
		if(elt.detachEvent){
			elt.detachEvent('on' + type, callback);
		}else if(elt.removeEventListener){
			elt.removeEventListener(type, callback, false);
		}else{
			elt['on'+type] = false;
		}
	};
	//

	return keyboardManagerService;
}]);
angular.module('umbraco.services')
.factory('localizationService', function ($http, $q, $rootScope, $window, $filter, userService) {
        var service = {
            // array to hold the localized resource string entries
            dictionary:[],
            // location of the resource file
            url: "js/language.aspx",
            // flag to indicate if the service hs loaded the resource file
            resourceFileLoaded:false,

            // success handler for all server communication
            successCallback:function (data) {
                // store the returned array in the dictionary
                service.dictionary = data;
                // set the flag that the resource are loaded
                service.resourceFileLoaded = true;
                // broadcast that the file has been loaded
                $rootScope.$broadcast('localizeResourcesUpdates');
            },

            // allows setting of language on the fly
            setLanguage: function(value) {
                service.initLocalizedResources();
            },

            // allows setting of resource url on the fly
            setUrl: function(value) {
                service.url = value;
                service.initLocalizedResources();
            },

            // loads the language resource file from the server
            initLocalizedResources:function () {
                var deferred = $q.defer();
                // build the url to retrieve the localized resource file
                $http({ method:"GET", url:service.url, cache:false })
                    .then(function(response){
                        service.resourceFileLoaded = true;
                        service.dictionary = response.data;

                        $rootScope.$broadcast('localizeResourcesUpdates');

                        return deferred.resolve(service.dictionary);
                    }, function(err){
                        return deferred.reject("Something broke");
                    });
                return deferred.promise;
            },

            //helper to tokenize and compile a localization string
            tokenize: function(value,scope) {
                    if(value){
                        var localizer = value.split(':');
                        var retval = {tokens: undefined, key: localizer[0].substring(0)};
                        if(localizer.length > 1){
                            retval.tokens = localizer[1].split(',');
                            for (var x = 0; x < retval.tokens.length; x++) {
                                retval.tokens[x] = scope.$eval(retval.tokens[x]);
                            }
                        }

                        return retval;
                    }
            },

            // checks the dictionary for a localized resource string
            localize: function(value,tokens) {
                var deferred = $q.defer();

                if(service.resourceFileLoaded){
                    var val = service._lookup(value,tokens);
                    deferred.resolve(val);
                }else{
                    service.initLocalizedResources().then(function(dic){
                           var val = service._lookup(value,tokens);
                           deferred.resolve(val); 
                    });
                }

                return deferred.promise;
            },
            _lookup: function(value,tokens){

                //strip the key identifier if its there
                if(value && value[0] === "@"){
                    value = value.substring(1);
                }

                //if no area specified, add general_
                if(value && value.indexOf("_") < 0){
                    value = "general_" + value;
                }

                var entry = service.dictionary[value];
                if(entry){
                    if(tokens){
                        for (var i = 0; i < tokens.length; i++) {
                            entry = entry.replace("%"+i+"%", tokens[i]);
                        }    
                    }
                    return entry;
                }
                return "[" + value + "]";
            }


        };

        // force the load of the resource file
        service.initLocalizedResources();

        // return the local instance when called
        return service;
    });
/**
 * @ngdoc service
 * @name umbraco.services.macroService
 *
 *  
 * @description
 * A service to return macro information such as generating syntax to insert a macro into an editor
 */
function macroService() {

    return {
        
        /** parses the special macro syntax like <?UMBRACO_MACRO macroAlias="Map" /> and returns an object with the macro alias and it's parameters */
        parseMacroSyntax: function (syntax) {

            var expression = /(<\?UMBRACO_MACRO macroAlias=["'](\w+?)["'].+?)(\/>|>.*?<\/\?UMBRACO_MACRO>)/im;
            var match = expression.exec(syntax);
            if (!match || match.length < 3) {
                return null;
            }
            var alias = match[2];

            //this will leave us with just the parameters
            var paramsChunk = match[1].trim().replace(new RegExp("UMBRACO_MACRO macroAlias=[\"']" + alias + "[\"']"), "").trim();
            
            var paramExpression = new RegExp("(\\w+?)=['\"](.*?)['\"]", "g");
            var paramMatch;
            var returnVal = {
                macroAlias: alias,
                marcoParamsDictionary: {}
            };
            while (paramMatch = paramExpression.exec(paramsChunk)) {
                returnVal.marcoParamsDictionary[paramMatch[1]] = paramMatch[2];
            }
            return returnVal;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.macroService#generateWebFormsSyntax
         * @methodOf umbraco.services.macroService
         * @function    
         *
         * @description
         * generates the syntax for inserting a macro into a rich text editor - this is the very old umbraco style syntax
         * 
         * @param {object} args an object containing the macro alias and it's parameter values
         */
        generateMacroSyntax: function (args) {

            // <?UMBRACO_MACRO macroAlias="BlogListPosts" />

            var macroString = '<?UMBRACO_MACRO macroAlias=\"' + args.macroAlias + "\" ";

            if (args.marcoParamsDictionary) {

                _.each(args.marcoParamsDictionary, function (val, key) {
                    var keyVal = key + "=\"" + (val ? val : "") + "\" ";
                    macroString += keyVal;
                });

            }

            macroString += "/>";

            return macroString;
        },

        /**
         * @ngdoc function
         * @name umbraco.services.macroService#generateWebFormsSyntax
         * @methodOf umbraco.services.macroService
         * @function    
         *
         * @description
         * generates the syntax for inserting a macro into a webforms templates
         * 
         * @param {object} args an object containing the macro alias and it's parameter values
         */
        generateWebFormsSyntax: function(args) {
            
            var macroString = '<umbraco:Macro ';

            if (args.marcoParamsDictionary) {
                
                _.each(args.marcoParamsDictionary, function (val, key) {
                    var keyVal = key + "=\"" + (val ? val : "") + "\" ";
                    macroString += keyVal;
                });

            }

            macroString += "Alias=\"" + args.macroAlias + "\" runat=\"server\"></umbraco:Macro>";

            return macroString;
        },
        
        /**
         * @ngdoc function
         * @name umbraco.services.macroService#generateMvcSyntax
         * @methodOf umbraco.services.macroService
         * @function    
         *
         * @description
         * generates the syntax for inserting a macro into an mvc template
         * 
         * @param {object} args an object containing the macro alias and it's parameter values
         */
        generateMvcSyntax: function (args) {

            var macroString = "@Umbraco.RenderMacro(\"" + args.macroAlias + "\"";

            var hasParams = false;
            var paramString;
            if (args.marcoParamsDictionary) {
                
                paramString = ", new {";

                _.each(args.marcoParamsDictionary, function(val, key) {

                    hasParams = true;
                    
                    var keyVal = key + "=\"" + (val ? val : "") + "\", ";

                    paramString += keyVal;
                });
                
                //remove the last , 
                paramString = paramString.trimEnd(", ");

                paramString += "}";
            }
            if (hasParams) {
                macroString += paramString;
            }

            macroString += ")";
            return macroString;
        }

    };

}

angular.module('umbraco.services').factory('macroService', macroService);
/**
 * @ngdoc service
 * @name umbraco.services.umbracoMenuActions
 *
 * @requires q
 * @requires treeService
 *	
 * @description
 * Defines the methods that are called when menu items declare only an action to execute
 */
function umbracoMenuActions($q, treeService, $location, navigationService, appState) {
    
    return {
        
        /**
         * @ngdoc method
         * @name umbraco.services.umbracoMenuActions#RefreshNode
         * @methodOf umbraco.services.umbracoMenuActions
         * @function
         *
         * @description
         * Clears all node children and then gets it's up-to-date children from the server and re-assigns them
         * @param {object} args An arguments object
         * @param {object} args.entity The basic entity being acted upon
         * @param {object} args.treeAlias The tree alias associated with this entity
         * @param {object} args.section The current section
         */
        "RefreshNode": function (args) {
            
            ////just in case clear any tree cache for this node/section
            //treeService.clearCache({
            //    cacheKey: "__" + args.section, //each item in the tree cache is cached by the section name
            //    childrenOf: args.entity.parentId //clear the children of the parent
            //});

            //since we're dealing with an entity, we need to attempt to find it's tree node, in the main tree
            // this action is purely a UI thing so if for whatever reason there is no loaded tree node in the UI
            // we can safely ignore this process.
            
            //to find a visible tree node, we'll go get the currently loaded root node from appState
            var treeRoot = appState.getTreeState("currentRootNode");
            if (treeRoot) {
                var treeNode = treeService.getDescendantNode(treeRoot, args.entity.id, args.treeAlias);
                if (treeNode) {
                    treeService.loadNodeChildren({ node: treeNode, section: args.section });
                }                
            }

            
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.umbracoMenuActions#CreateChildEntity
         * @methodOf umbraco.services.umbracoMenuActions
         * @function
         *
         * @description
         * This will re-route to a route for creating a new entity as a child of the current node
         * @param {object} args An arguments object
         * @param {object} args.entity The basic entity being acted upon
         * @param {object} args.treeAlias The tree alias associated with this entity
         * @param {object} args.section The current section
         */
        "CreateChildEntity": function (args) {

            navigationService.hideNavigation();

            var route = "/" + args.section + "/" + args.treeAlias + "/edit/" + args.entity.id;
            //change to new path
            $location.path(route).search({ create: true });
            
        }
    };
} 

angular.module('umbraco.services').factory('umbracoMenuActions', umbracoMenuActions);
/**
 * @ngdoc service
 * @name umbraco.services.navigationService
 *
 * @requires $rootScope 
 * @requires $routeParams
 * @requires $log
 * @requires $location
 * @requires dialogService
 * @requires treeService
 * @requires sectionResource
 *	
 * @description
 * Service to handle the main application navigation. Responsible for invoking the tree
 * Section navigation and search, and maintain their state for the entire application lifetime
 *
 */
function navigationService($rootScope, $routeParams, $log, $location, $q, $timeout, $injector, dialogService, umbModelMapper, treeService, notificationsService, historyService, appState, angularHelper) {

    var minScreenSize = 1100;
    //used to track the current dialog object
    var currentDialog = null;
    //tracks the screen size as a tablet
    var isTablet = false;
    //the main tree event handler, which gets assigned via the setupTreeEvents method
    var mainTreeEventHandler = null;
    //tracks the user profile dialog
    var userDialog = null;

    function setTreeMode() {
        isTablet = ($(window).width() <= minScreenSize);
        appState.setGlobalState("showNavigation", !isTablet);
    }

    function setMode(mode) {
        switch (mode) {
        case 'tree':
            appState.setGlobalState("navMode", "tree");
            appState.setGlobalState("showNavigation", true);
            appState.setMenuState("showMenu", false);
            appState.setMenuState("showMenuDialog", false);
            appState.setGlobalState("stickyNavigation", false);
            appState.setGlobalState("showTray", false);
            
            //$("#search-form input").focus();    
            break;
        case 'menu':
            appState.setGlobalState("navMode", "menu");
            appState.setGlobalState("showNavigation", true);
            appState.setMenuState("showMenu", true);
            appState.setMenuState("showMenuDialog", false);
            appState.setGlobalState("stickyNavigation", true);
            break;
        case 'dialog':
            appState.setGlobalState("navMode", "dialog");
            appState.setGlobalState("stickyNavigation", true);
            appState.setGlobalState("showNavigation", true);
            appState.setMenuState("showMenu", false);
            appState.setMenuState("showMenuDialog", true);
            break;
        case 'search':
            appState.setGlobalState("navMode", "search");
            appState.setGlobalState("stickyNavigation", false);
            appState.setGlobalState("showNavigation", true);
            appState.setMenuState("showMenu", false);
            appState.setSectionState("showSearchResults", true);
            appState.setMenuState("showMenuDialog", false);

            //TODO: This would be much better off in the search field controller listening to appState changes
            $timeout(function() {
                $("#search-field").focus();
            });

            break;
        default:
            appState.setGlobalState("navMode", "default");
            appState.setMenuState("showMenu", false);
            appState.setMenuState("showMenuDialog", false);
            appState.setSectionState("showSearchResults", false);
            appState.setGlobalState("stickyNavigation", false);
            appState.setGlobalState("showTray", false);

            if (isTablet) {
                appState.setGlobalState("showNavigation", false);
            }

            break;
        }
    }

    var service = {

        /** initializes the navigation service */
        init: function() {

            setTreeMode();
            
            //keep track of the current section - initially this will always be undefined so 
            // no point in setting it now until it changes.
            $rootScope.$watch(function () {
                return $routeParams.section;
            }, function (newVal, oldVal) {
                appState.setSectionState("currentSection", newVal);
            });

            //TODO: This does not belong here - would be much better off in a directive
            $(window).bind("resize", function() {
                setTreeMode();
            });
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#load
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Shows the legacy iframe and loads in the content based on the source url
         * @param {String} source The URL to load into the iframe
         */
        loadLegacyIFrame: function (source) {            
            $location.path("/" + appState.getSectionState("currentSection") + "/framed/" + encodeURIComponent(source));
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#changeSection
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Changes the active section to a given section alias
         * If the navigation is 'sticky' this will load the associated tree
         * and load the dashboard related to the section
         * @param {string} sectionAlias The alias of the section
         */
        changeSection: function(sectionAlias, force) {
            setMode("default-opensection");

            if (force && appState.getSectionState("currentSection") === sectionAlias) {
                appState.setSectionState("currentSection", "");
            }

            appState.setSectionState("currentSection", sectionAlias);
            this.showTree(sectionAlias);

            $location.path(sectionAlias);
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#showTree
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Displays the tree for a given section alias but turning on the containing dom element
         * only changes if the section is different from the current one
		 * @param {string} sectionAlias The alias of the section to load
         * @param {Object} syncArgs Optional object of arguments for syncing the tree for the section being shown
		 */
        showTree: function (sectionAlias, syncArgs) {
            if (sectionAlias !== appState.getSectionState("currentSection")) {
                appState.setSectionState("currentSection", sectionAlias);
                
                if (syncArgs) {
                    this.syncTree(syncArgs);
                }
            }
            setMode("tree");
        },

        showTray: function () {
            appState.setGlobalState("showTray", true);
        },

        hideTray: function () {
            appState.setGlobalState("showTray", false);
        },

        /** 
            Called to assign the main tree event handler - this is called by the navigation controller.
            TODO: Potentially another dev could call this which would kind of mung the whole app so potentially there's a better way.
        */
        setupTreeEvents: function(treeEventHandler) {
            mainTreeEventHandler = treeEventHandler;

            //when a tree is loaded into a section, we need to put it into appState
            mainTreeEventHandler.bind("treeLoaded", function(ev, args) {
                appState.setTreeState("currentRootNode", args.tree);
            });

            //when a tree node is synced this event will fire, this allows us to set the currentNode
            mainTreeEventHandler.bind("treeSynced", function (ev, args) {
                
                if (args.activate === undefined || args.activate === true) {
                    //set the current selected node
                    appState.setTreeState("selectedNode", args.node);
                    //when a node is activated, this is the same as clicking it and we need to set the
                    //current menu item to be this node as well.
                    appState.setMenuState("currentNode", args.node);
                }
            });

            //this reacts to the options item in the tree
            mainTreeEventHandler.bind("treeOptionsClick", function(ev, args) {
                ev.stopPropagation();
                ev.preventDefault();

                //Set the current action node (this is not the same as the current selected node!)
                appState.setMenuState("currentNode", args.node);
                
                if (args.event && args.event.altKey) {
                    args.skipDefault = true;
                }

                service.showMenu(ev, args);
            });

            mainTreeEventHandler.bind("treeNodeAltSelect", function(ev, args) {
                ev.stopPropagation();
                ev.preventDefault();

                args.skipDefault = true;
                service.showMenu(ev, args);
            });

            //this reacts to tree items themselves being clicked
            //the tree directive should not contain any handling, simply just bubble events
            mainTreeEventHandler.bind("treeNodeSelect", function(ev, args) {
                var n = args.node;
                ev.stopPropagation();
                ev.preventDefault();

                if (n.metaData && n.metaData["jsClickCallback"] && angular.isString(n.metaData["jsClickCallback"]) && n.metaData["jsClickCallback"] !== "") {
                    //this is a legacy tree node!                
                    var jsPrefix = "javascript:";
                    var js;
                    if (n.metaData["jsClickCallback"].startsWith(jsPrefix)) {
                        js = n.metaData["jsClickCallback"].substr(jsPrefix.length);
                    }
                    else {
                        js = n.metaData["jsClickCallback"];
                    }
                    try {
                        var func = eval(js);
                        //this is normally not necessary since the eval above should execute the method and will return nothing.
                        if (func != null && (typeof func === "function")) {
                            func.call();
                        }
                    }
                    catch(ex) {
                        $log.error("Error evaluating js callback from legacy tree node: " + ex);
                    }
                }
                else if (n.routePath) {
                    //add action to the history service
                    historyService.add({ name: n.name, link: n.routePath, icon: n.icon });
                    
                    //put this node into the tree state
                    appState.setTreeState("selectedNode", args.node);
                    //when a node is clicked we also need to set the active menu node to this node
                    appState.setMenuState("currentNode", args.node);

                    //not legacy, lets just set the route value and clear the query string if there is one.
                    $location.path(n.routePath).search("");
                }
                else if (args.element.section) {
                    $location.path(args.element.section).search("");
                }

                service.hideNavigation();
            });
        },
        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#syncTree
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Syncs a tree with a given path, returns a promise
         * The path format is: ["itemId","itemId"], and so on
         * so to sync to a specific document type node do:
         * <pre>
         * navigationService.syncTree({tree: 'content', path: ["-1","123d"], forceReload: true});  
         * </pre>
         * @param {Object} args arguments passed to the function
         * @param {String} args.tree the tree alias to sync to
         * @param {Array} args.path the path to sync the tree to
         * @param {Boolean} args.forceReload optional, specifies whether to force reload the node data from the server even if it already exists in the tree currently
         * @param {Boolean} args.activate optional, specifies whether to set the synced node to be the active node, this will default to true if not specified
         */
        syncTree: function (args) {
            if (!args) {
                throw "args cannot be null";
            }
            if (!args.path) {
                throw "args.path cannot be null";
            }
            if (!args.tree) {
                throw "args.tree cannot be null";
            }
            
            if (mainTreeEventHandler) {
                //returns a promise
                return mainTreeEventHandler.syncTree(args);
            }

            //couldn't sync
            return angularHelper.rejectedPromise();
        },

        /** 
            Internal method that should ONLY be used by the legacy API wrapper, the legacy API used to 
            have to set an active tree and then sync, the new API does this in one method by using syncTree
        */
        _syncPath: function(path, forceReload) {
            if (mainTreeEventHandler) {
                mainTreeEventHandler.syncTree({ path: path, forceReload: forceReload });
            }
        },

        //TODO: This should return a promise
        reloadNode: function(node) {
            if (mainTreeEventHandler) {
                mainTreeEventHandler.reloadNode(node);
            }
        },

        //TODO: This should return a promise
        reloadSection: function(sectionAlias) {
            if (mainTreeEventHandler) {
                mainTreeEventHandler.clearCache({ section: sectionAlias });
                mainTreeEventHandler.load(sectionAlias);
            }
        },

        /** 
            Internal method that should ONLY be used by the legacy API wrapper, the legacy API used to 
            have to set an active tree and then sync, the new API does this in one method by using syncTreePath
        */
        _setActiveTreeType: function (treeAlias, loadChildren) {
            if (mainTreeEventHandler) {
                mainTreeEventHandler._setActiveTreeType(treeAlias, loadChildren);
            }
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#hideTree
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Hides the tree by hiding the containing dom element
         */
        hideTree: function() {

            if (isTablet && !appState.getGlobalState("stickyNavigation")) {
                //reset it to whatever is in the url
                appState.setSectionState("currentSection", $routeParams.section);
                setMode("default-hidesectiontree");
            }

        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#showMenu
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Hides the tree by hiding the containing dom element. 
         * This always returns a promise!
         *
         * @param {Event} event the click event triggering the method, passed from the DOM element
         */
        showMenu: function(event, args) {

            var deferred = $q.defer();
            var self = this;

            treeService.getMenu({ treeNode: args.node })
                .then(function(data) {

                    //check for a default
                    //NOTE: event will be undefined when a call to hideDialog is made so it won't re-load the default again.
                    // but perhaps there's a better way to deal with with an additional parameter in the args ? it works though.
                    if (data.defaultAlias && !args.skipDefault) {

                        var found = _.find(data.menuItems, function(item) {
                            return item.alias = data.defaultAlias;
                        });

                        if (found) {

                            //NOTE: This is assigning the current action node - this is not the same as the currently selected node!
                            appState.setMenuState("currentNode", args.node);
                            
                            //ensure the current dialog is cleared before creating another!
                            if (currentDialog) {
                                dialogService.close(currentDialog);
                            }

                            var dialog = self.showDialog({
                                scope: args.scope,
                                node: args.node,
                                action: found,
                                section: appState.getSectionState("currentSection")
                            });

                            //return the dialog this is opening.
                            deferred.resolve(dialog);
                            return;
                        }
                    }

                    //there is no default or we couldn't find one so just continue showing the menu                    

                    setMode("menu");

                    appState.setMenuState("currentNode", args.node);
                    appState.setMenuState("menuActions", data.menuItems);
                    appState.setMenuState("dialogTitle", args.node.name);                    

                    //we're not opening a dialog, return null.
                    deferred.resolve(null);
                });

            return deferred.promise;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#hideMenu
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Hides the menu by hiding the containing dom element
         */
        hideMenu: function() {
            //SD: Would we ever want to access the last action'd node instead of clearing it here?
            appState.setMenuState("currentNode", null);
            appState.setMenuState("menuActions", []);
            setMode("tree");
        },

        /** Executes a given menu action */
        executeMenuAction: function (action, node, section) {

            if (!action) {
                throw "action cannot be null";
            }
            if (!node) {
                throw "node cannot be null";                
            }
            if (!section) {
                throw "section cannot be null";
            }

            if (action.metaData && action.metaData["actionRoute"] && angular.isString(action.metaData["actionRoute"])) {
                //first check if the menu item simply navigates to a route
                var parts = action.metaData["actionRoute"].split("?");
                $location.path(parts[0]).search(parts.length > 1 ? parts[1] : "");
                this.hideNavigation();
                return;
            }
            else if (action.metaData && action.metaData["jsAction"] && angular.isString(action.metaData["jsAction"])) {

                //we'll try to get the jsAction from the injector
                var menuAction = action.metaData["jsAction"].split('.');
                if (menuAction.length !== 2) {

                    //if it is not two parts long then this most likely means that it's a legacy action                         
                    var js = action.metaData["jsAction"].replace("javascript:", "");
                    //there's not really a different way to acheive this except for eval 
                    eval(js);
                }
                else {
                    var menuActionService = $injector.get(menuAction[0]);
                    if (!menuActionService) {
                        throw "The angular service " + menuAction[0] + " could not be found";
                    }

                    var method = menuActionService[menuAction[1]];

                    if (!method) {
                        throw "The method " + menuAction[1] + " on the angular service " + menuAction[0] + " could not be found";
                    }

                    method.apply(this, [{
                        //map our content object to a basic entity to pass in to the menu handlers,
                        //this is required for consistency since a menu item needs to be decoupled from a tree node since the menu can
                        //exist standalone in the editor for which it can only pass in an entity (not tree node).
                        entity: umbModelMapper.convertToEntityBasic(node),
                        action: action,
                        section: section,
                        treeAlias: treeService.getTreeAlias(node)
                    }]);
                }
            }
            else {
                service.showDialog({
                    node: node,
                    action: action,
                    section: section
                });
            }
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#showUserDialog
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Opens the user dialog, next to the sections navigation
         * template is located in views/common/dialogs/user.html
         */
        showUserDialog: function() {

            if(userDialog){
                userDialog.close();
                userDialog = null;
            }

            userDialog = dialogService.open(
                {
                    template: "views/common/dialogs/user.html",
                    modalClass: "umb-modal-left",
                    show: true
                });
        
            

            return userDialog;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#showUserDialog
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Opens the user dialog, next to the sections navigation
         * template is located in views/common/dialogs/user.html
         */
        showHelpDialog: function() {
            if(service.helpDialog){
                service.helpDialog.close();
                service.helpDialog = undefined;
            }

            service.helpDialog = dialogService.open(
            {
                template: "views/common/dialogs/help.html",
                modalClass: "umb-modal-left",
                show: true
            });
            
            return service.helpDialog;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.navigationService#showDialog
         * @methodOf umbraco.services.navigationService
         *
         * @description
         * Opens a dialog, for a given action on a given tree node
         * uses the dialogService to inject the selected action dialog
         * into #dialog div.umb-panel-body
         * the path to the dialog view is determined by: 
         * "views/" + current tree + "/" + action alias + ".html"
         * The dialog controller will get passed a scope object that is created here. This scope
         * object may be injected as part of the args object, if one is not found then a new scope
         * is created. Regardless of whether a scope is created or re-used, a few properties and methods 
         * will be added to it so that they can be used in any dialog controller:
         *  scope.currentNode = the selected tree node
         *  scope.currentAction = the selected menu item
         * @param {Object} args arguments passed to the function
         * @param {Scope} args.scope current scope passed to the dialog
         * @param {Object} args.action the clicked action containing `name` and `alias`
         */
        showDialog: function(args) {

            if (!args) {
                throw "showDialog is missing the args parameter";
            }
            if (!args.action) {
                throw "The args parameter must have an 'action' property as the clicked menu action object";
            }
            if (!args.node) {
                throw "The args parameter must have a 'node' as the active tree node";
            }

            //ensure the current dialog is cleared before creating another!
            if (currentDialog) {
                dialogService.close(currentDialog);
            }

            setMode("dialog");

            //set up the scope object and assign properties
            var dialogScope = args.scope || $rootScope.$new();
            dialogScope.currentNode = args.node;
            dialogScope.currentAction = args.action;

            //the title might be in the meta data, check there first
            if (args.action.metaData["dialogTitle"]) {
                appState.setMenuState("dialogTitle", args.action.metaData["dialogTitle"]);
            }
            else {
                appState.setMenuState("dialogTitle", args.action.name);
            }

            var templateUrl;
            var iframe;

            if (args.action.metaData["actionUrl"]) {
                templateUrl = args.action.metaData["actionUrl"];
                iframe = true;
            }
            else if (args.action.metaData["actionView"]) {
                templateUrl = args.action.metaData["actionView"];
                iframe = false;
            }
            else {

                //by convention we will look into the /views/{treetype}/{action}.html
                // for example: /views/content/create.html

                //we will also check for a 'packageName' for the current tree, if it exists then the convention will be:
                // for example: /App_Plugins/{mypackage}/umbraco/{treetype}/create.html

                var treeAlias = treeService.getTreeAlias(args.node);
                var packageTreeFolder = treeService.getTreePackageFolder(treeAlias);

                if (!treeAlias) {
                    throw "Could not get tree alias for node " + args.node.id;
                }

                if (packageTreeFolder) {
                    templateUrl = Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath +
                        "/" + packageTreeFolder +
                        "/umbraco/" + treeAlias + "/" + args.action.alias + ".html";
                }
                else {
                    templateUrl = "views/" + treeAlias + "/" + args.action.alias + ".html";
                }

                iframe = false;
            }

            //TODO: some action's want to launch a new window like live editing, we support this in the menu item's metadata with
            // a key called: "actionUrlMethod" which can be set to either: Dialog, BlankWindow. Normally this is always set to Dialog 
            // if a URL is specified in the "actionUrl" metadata. For now I'm not going to implement launching in a blank window, 
            // though would be v-easy, just not sure we want to ever support that?

            var dialog = dialogService.open(
                {
                    container: $("#dialog div.umb-modalcolumn-body"),
                    scope: dialogScope,
                    currentNode: args.node,
                    currentAction: args.action,
                    inline: true,
                    show: true,
                    iframe: iframe,
                    modalClass: "umb-dialog",
                    template: templateUrl
                });

            //save the currently assigned dialog so it can be removed before a new one is created
            currentDialog = dialog;
            return dialog;
        },

        /**
	     * @ngdoc method
	     * @name umbraco.services.navigationService#hideDialog
	     * @methodOf umbraco.services.navigationService
	     *
	     * @description
	     * hides the currently open dialog
	     */
        hideDialog: function () {
            this.showMenu(undefined, { skipDefault: true, node: appState.getMenuState("currentNode") });
        },
        /**
          * @ngdoc method
          * @name umbraco.services.navigationService#showSearch
          * @methodOf umbraco.services.navigationService
          *
          * @description
          * shows the search pane
          */
        showSearch: function() {
            setMode("search");
        },
        /**
          * @ngdoc method
          * @name umbraco.services.navigationService#hideSearch
          * @methodOf umbraco.services.navigationService
          *
          * @description
          * hides the search pane
        */
        hideSearch: function() {
            setMode("default-hidesearch");
        },
        /**
          * @ngdoc method
          * @name umbraco.services.navigationService#hideNavigation
          * @methodOf umbraco.services.navigationService
          *
          * @description
          * hides any open navigation panes and resets the tree, actions and the currently selected node
          */
        hideNavigation: function() {
            appState.setMenuState("menuActions", []);
            setMode("default");
        }
    };

    return service;
}

angular.module('umbraco.services').factory('navigationService', navigationService);

/**
 * @ngdoc service
 * @name umbraco.services.notificationsService
 *
 * @requires $rootScope 
 * @requires $timeout
 * @requires angularHelper
 *	
 * @description
 * Application-wide service for handling notifications, the umbraco application 
 * maintains a single collection of notications, which the UI watches for changes.
 * By default when a notication is added, it is automaticly removed 7 seconds after
 * This can be changed on add()
 *
 * ##usage
 * To use, simply inject the notificationsService into any controller that needs it, and make
 * sure the umbraco.services module is accesible - which it should be by default.
 *
 * <pre>
 *		notificationsService.success("Document Published", "hooraaaay for you!");
 *      notificationsService.error("Document Failed", "booooh");
 * </pre> 
 */
angular.module('umbraco.services')
.factory('notificationsService', function ($rootScope, $timeout, angularHelper) {

	var nArray = [];

	var service = {

		/**
		* @ngdoc method
		* @name umbraco.services.notificationsService#add
		* @methodOf umbraco.services.notificationsService
		*
		* @description
		* Lower level api for adding notifcations, support more advanced options
		* @param {Object} item The notification item
		* @param {String} item.headline Short headline
		* @param {String} item.message longer text for the notication, trimmed after 200 characters, which can then be exanded
		* @param {String} item.type Notification type, can be: "success","warning","error" or "info" 
		* @param {String} item.url url to open when notification is clicked
		* @param {Boolean} item.sticky if set to true, the notification will not auto-close
		* @returns {Object} args notification object
		*/

		add: function(item) {
			angularHelper.safeApply($rootScope, function () {

				//add a colon after the headline if there is a message as well
				if (item.message) {
					item.headline += ":";
					if(item.message.length > 200) {
						item.sticky = true;
					}
				}

				//we need to ID the item, going by index isn't good enough because people can remove at different indexes 
				// whenever they want. Plus once we remove one, then the next index will be different. The only way to 
				// effectively remove an item is by an Id.
				item.id = String.CreateGuid();

				nArray.push(item);

				if(!item.sticky) {
					$timeout(function() {
						var found = _.find(nArray, function(i) {
						return i.id === item.id;
					});

					if (found) {
						var index = nArray.indexOf(found);
						nArray.splice(index, 1);
					}

					}, 7000);
				}

				return item;
			});

		},

	    /**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#showNotification
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Shows a notification based on the object passed in, normally used to render notifications sent back from the server
		 *		 
		 * @returns {Object} args notification object
		 */
        showNotification: function(args) {
            if (!args) {
                throw "args cannot be null";
            }
            if (args.type === undefined || args.type === null) {
                throw "args.type cannot be null";
            }
            if (!args.header) {
                throw "args.header cannot be null";
            }
            
            switch(args.type) {
                case 0:
                    //save
                    this.success(args.header, args.message);
                    break;
                case 1:
                    //info
                    this.success(args.header, args.message);
                    break;
                case 2:
                    //error
                    this.error(args.header, args.message);
                    break;
                case 3:
                    //success
                    this.success(args.header, args.message);
                    break;
                case 4:
                    //warning
                    this.warning(args.header, args.message);
                    break;
            }
        },

	    /**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#success
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Adds a green success notication to the notications collection
		 * This should be used when an operations *completes* without errors
		 *
		 * @param {String} headline Headline of the notification
		 * @param {String} message longer text for the notication, trimmed after 200 characters, which can then be exanded
		 * @returns {Object} notification object
		 */
	    success: function (headline, message) {
	        return service.add({ headline: headline, message: message, type: 'success', time: new Date() });
	    },
		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#error
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Adds a red error notication to the notications collection
		 * This should be used when an operations *fails* and could not complete
		 * 
		 * @param {String} headline Headline of the notification
		 * @param {String} message longer text for the notication, trimmed after 200 characters, which can then be exanded
		 * @returns {Object} notification object
		 */
	    error: function (headline, message) {
	        return service.add({ headline: headline, message: message, type: 'error', time: new Date() });
		},

		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#warning
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Adds a yellow warning notication to the notications collection
		 * This should be used when an operations *completes* but something was not as expected
		 * 
		 *
		 * @param {String} headline Headline of the notification
		 * @param {String} message longer text for the notication, trimmed after 200 characters, which can then be exanded
		 * @returns {Object} notification object
		 */
	    warning: function (headline, message) {
	        return service.add({ headline: headline, message: message, type: 'warning', time: new Date() });
		},

		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#warning
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Adds a yellow warning notication to the notications collection
		 * This should be used when an operations *completes* but something was not as expected
		 * 
		 *
		 * @param {String} headline Headline of the notification
		 * @param {String} message longer text for the notication, trimmed after 200 characters, which can then be exanded
		 * @returns {Object} notification object
		 */
	    info: function (headline, message) {
	        return service.add({ headline: headline, message: message, type: 'info', time: new Date() });
		},

		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#remove
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Removes a notification from the notifcations collection at a given index 
		 *
		 * @param {Int} index index where the notication should be removed from
		 */
	    remove: function (index) {
	        angularHelper.safeApply($rootScope, function() {
	            nArray.splice(index, 1);
	        });
		},

		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#removeAll
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Removes all notifications from the notifcations collection 
		 */
	    removeAll: function () {
	        angularHelper.safeApply($rootScope, function() {
	            nArray = [];
	        });
		},

		/**
		 * @ngdoc property
		 * @name umbraco.services.notificationsService#current
		 * @propertyOf umbraco.services.notificationsService
		 *
		 * @description
		 * Returns an array of current notifications to display
		 *
		 * @returns {string} returns an array
		 */
		current: nArray,

		/**
		 * @ngdoc method
		 * @name umbraco.services.notificationsService#getCurrent
		 * @methodOf umbraco.services.notificationsService
		 *
		 * @description
		 * Method to return all notifications from the notifcations collection 
		 */
		getCurrent: function(){
			return nArray;
		}
	};

	return service;
});
/**
 * @ngdoc service
 * @name umbraco.services.searchService
 *
 *  
 * @description
 * Service for handling the main application search, can currently search content, media and members
 *
 * ##usage
 * To use, simply inject the searchService into any controller that needs it, and make
 * sure the umbraco.services module is accesible - which it should be by default.
 *
 * <pre>
 *      searchService.searchMembers({term: 'bob'}).then(function(results){
 *          angular.forEach(results, function(result){
 *                  //returns:
 *                  {name: "name", id: 1234, menuUrl: "url", editorPath: "url", metaData: {}, subtitle: "/path/etc" }
 *           })          
 *           var result = 
 *       }) 
 * </pre> 
 */
angular.module('umbraco.services')
.factory('searchService', function ($q, $log, entityResource, contentResource, umbRequestHelper) {

    function configureMemberResult(member) {
        member.menuUrl = umbRequestHelper.getApiUrl("memberTreeBaseUrl", "GetMenu", [{ id: member.id }, { application: 'member' }]);
        member.editorPath = "member/member/edit/" + (member.key ? member.key : member.id);
        member.metaData = { treeAlias: "member" };
        member.subTitle = member.metaData.Email;
    }
    
    function configureMediaResult(media)
    {
        media.menuUrl = umbRequestHelper.getApiUrl("mediaTreeBaseUrl", "GetMenu", [{ id: media.id }, { application: 'media' }]);
        media.editorPath = "media/media/edit/" + media.id;
        media.metaData = { treeAlias: "media" };
    }
    
    function configureContentResult(content) {
        content.menuUrl = umbRequestHelper.getApiUrl("contentTreeBaseUrl", "GetMenu", [{ id: content.id }, { application: 'content' }]);
        content.editorPath = "content/content/edit/" + content.id;
        content.metaData = { treeAlias: "content" };
        content.subTitle = content.metaData.Url;        
    }

    return {

        /**
        * @ngdoc method
        * @name umbraco.services.searchService#searchMembers
        * @methodOf umbraco.services.searchService
        *
        * @description
        * Searches the default member search index
        * @param {Object} args argument object
        * @param {String} args.term seach term
        * @returns {Promise} returns promise containing all matching members
        */
        searchMembers: function(args) {

            if (!args.term) {
                throw "args.term is required";
            }

            return entityResource.search(args.term, "Member").then(function (data) {
                _.each(data, function(item) {
                    configureMemberResult(item);
                });         
                return data;
            });
        },

        /**
        * @ngdoc method
        * @name umbraco.services.searchService#searchContent
        * @methodOf umbraco.services.searchService
        *
        * @description
        * Searches the default internal content search index
        * @param {Object} args argument object
        * @param {String} args.term seach term
        * @returns {Promise} returns promise containing all matching content items
        */
        searchContent: function(args) {

            if (!args.term) {
                throw "args.term is required";
            }

            return entityResource.search(args.term, "Document").then(function (data) {
                _.each(data, function (item) {
                    configureContentResult(item);
                });
                return data;
            });
        },

        /**
        * @ngdoc method
        * @name umbraco.services.searchService#searchMedia
        * @methodOf umbraco.services.searchService
        *
        * @description
        * Searches the default media search index
        * @param {Object} args argument object
        * @param {String} args.term seach term
        * @returns {Promise} returns promise containing all matching media items
        */
        searchMedia: function(args) {

            if (!args.term) {
                throw "args.term is required";
            }

            return entityResource.search(args.term, "Media").then(function (data) {
                _.each(data, function (item) {
                    configureMediaResult(item);
                });
                return data;
            });
        },

        /**
        * @ngdoc method
        * @name umbraco.services.searchService#searchAll
        * @methodOf umbraco.services.searchService
        *
        * @description
        * Searches all available indexes and returns all results in one collection
        * @param {Object} args argument object
        * @param {String} args.term seach term
        * @returns {Promise} returns promise containing all matching items
        */
        searchAll: function (args) {
            
            if (!args.term) {
                throw "args.term is required";
            }

            return entityResource.searchAll(args.term).then(function (data) {

                _.each(data, function(resultByType) {
                    switch(resultByType.type) {
                        case "Document":
                            _.each(resultByType.results, function (item) {
                                configureContentResult(item);
                            });
                            break;
                        case "Media":
                            _.each(resultByType.results, function (item) {
                                configureMediaResult(item);
                            });                            
                            break;
                        case "Member":
                            _.each(resultByType.results, function (item) {
                                configureMemberResult(item);
                            });                            
                            break;
                    }
                });

                return data;
            });
            
        },

        setCurrent: function(sectionAlias) {
            currentSection = sectionAlias;
        }
    };
});
/**
 * @ngdoc service
 * @name umbraco.services.serverValidationManager
 * @function
 *
 * @description
 * Used to handle server side validation and wires up the UI with the messages. There are 2 types of validation messages, one
 * is for user defined properties (called Properties) and the other is for field properties which are attached to the native 
 * model objects (not user defined). The methods below are named according to these rules: Properties vs Fields.
 */
function serverValidationManager($timeout) {

    var callbacks = [];
    
    /** calls the callback specified with the errors specified, used internally */
    function executeCallback(self, errorsForCallback, callback) {

        callback.apply(self, [
                 false,                  //pass in a value indicating it is invalid
                 errorsForCallback,      //pass in the errors for this item
                 self.items]);           //pass in all errors in total
    }

    function getFieldErrors(self, fieldName) {
        if (!angular.isString(fieldName)) {
            throw "fieldName must be a string";
        }

        //find errors for this field name
        return _.filter(self.items, function (item) {
            return (item.propertyAlias === null && item.fieldName === fieldName);
        });
    }
    
    function getPropertyErrors(self, propertyAlias, fieldName) {
        if (!angular.isString(propertyAlias)) {
            throw "propertyAlias must be a string";
        }
        if (fieldName && !angular.isString(fieldName)) {
            throw "fieldName must be a string";
        }

        //find all errors for this property
        return _.filter(self.items, function (item) {            
            return (item.propertyAlias === propertyAlias && (item.fieldName === fieldName || (fieldName === undefined || fieldName === "")));
        });
    }

    return {
        
        /**
         * @ngdoc function
         * @name umbraco.services.serverValidationManager#subscribe
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         *  This method needs to be called once all field and property errors are wired up. 
         * 
         *  In some scenarios where the error collection needs to be persisted over a route change 
         *   (i.e. when a content item (or any item) is created and the route redirects to the editor) 
         *   the controller should call this method once the data is bound to the scope
         *   so that any persisted validation errors are re-bound to their controls. Once they are re-binded this then clears the validation
         *   colleciton so that if another route change occurs, the previously persisted validation errors are not re-bound to the new item.
         */
        executeAndClearAllSubscriptions: function() {

            var self = this;

            $timeout(function () {
                
                for (var cb in callbacks) {
                    if (callbacks[cb].propertyAlias === null) {
                        //its a field error callback
                        var fieldErrors = getFieldErrors(self, callbacks[cb].fieldName);
                        if (fieldErrors.length > 0) {
                            executeCallback(self, fieldErrors, callbacks[cb].callback);
                        }
                    }
                    else {
                        //its a property error
                        var propErrors = getPropertyErrors(self, callbacks[cb].propertyAlias, callbacks[cb].fieldName);
                        if (propErrors.length > 0) {
                            executeCallback(self, propErrors, callbacks[cb].callback);
                        }
                    }
                }
                //now that they are all executed, we're gonna clear all of the errors we have
                self.clear();
            });
        },

        /**
         * @ngdoc function
         * @name umbraco.services.serverValidationManager#subscribe
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         *  Adds a callback method that is executed whenever validation changes for the field name + property specified.
         *  This is generally used for server side validation in order to match up a server side validation error with 
         *  a particular field, otherwise we can only pinpoint that there is an error for a content property, not the 
         *  property's specific field. This is used with the val-server directive in which the directive specifies the 
         *  field alias to listen for.
         *  If propertyAlias is null, then this subscription is for a field property (not a user defined property).
         */
        subscribe: function (propertyAlias, fieldName, callback) {
            if (!callback) {
                return;
            }
            
            if (propertyAlias === null) {
                //don't add it if it already exists
                var exists1 = _.find(callbacks, function (item) {
                    return item.propertyAlias === null && item.fieldName === fieldName;
                });
                if (!exists1) {
                    callbacks.push({ propertyAlias: null, fieldName: fieldName, callback: callback });
                }
            }
            else if (propertyAlias !== undefined) {
                //don't add it if it already exists
                var exists2 = _.find(callbacks, function (item) {
                    return item.propertyAlias === propertyAlias && item.fieldName === fieldName;
                });
                if (!exists2) {
                    callbacks.push({ propertyAlias: propertyAlias, fieldName: fieldName, callback: callback });
                }
            }
        },
        
        unsubscribe: function (propertyAlias, fieldName) {
            
            if (propertyAlias === null) {

                //remove all callbacks for the content field
                callbacks = _.reject(callbacks, function (item) {
                    return item.propertyAlias === null && item.fieldName === fieldName;
                });

            }
            else if (propertyAlias !== undefined) {
                
                //remove all callbacks for the content property
                callbacks = _.reject(callbacks, function (item) {
                    return item.propertyAlias === propertyAlias &&
                    (item.fieldName === fieldName ||
                        ((item.fieldName === undefined || item.fieldName === "") && (fieldName === undefined || fieldName === "")));
                });
            }

            
        },
        
        
        /**
         * @ngdoc function
         * @name getPropertyCallbacks
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Gets all callbacks that has been registered using the subscribe method for the propertyAlias + fieldName combo.
         * This will always return any callbacks registered for just the property (i.e. field name is empty) and for ones with an 
         * explicit field name set.
         */
        getPropertyCallbacks: function (propertyAlias, fieldName) {
            var found = _.filter(callbacks, function (item) {
                //returns any callback that have been registered directly against the field and for only the property
                return (item.propertyAlias === propertyAlias && (item.fieldName === fieldName || (item.fieldName === undefined || item.fieldName === "")));
            });
            return found;
        },
        
        /**
         * @ngdoc function
         * @name getFieldCallbacks
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Gets all callbacks that has been registered using the subscribe method for the field.         
         */
        getFieldCallbacks: function (fieldName) {
            var found = _.filter(callbacks, function (item) {
                //returns any callback that have been registered directly against the field
                return (item.propertyAlias === null && item.fieldName === fieldName);
            });
            return found;
        },
        
        /**
         * @ngdoc function
         * @name addFieldError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Adds an error message for a native content item field (not a user defined property, for Example, 'Name')
         */
        addFieldError: function(fieldName, errorMsg) {
            if (!fieldName) {
                return;
            }
            
            //only add the item if it doesn't exist                
            if (!this.hasFieldError(fieldName)) {
                this.items.push({
                    propertyAlias: null,
                    fieldName: fieldName,
                    errorMsg: errorMsg
                });
            }
            
            //find all errors for this item
            var errorsForCallback = getFieldErrors(this, fieldName);
            //we should now call all of the call backs registered for this error
            var cbs = this.getFieldCallbacks(fieldName);
            //call each callback for this error
            for (var cb in cbs) {
                executeCallback(this, errorsForCallback, cbs[cb].callback);
            }
        },

        /**
         * @ngdoc function
         * @name addPropertyError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Adds an error message for the content property
         */
        addPropertyError: function (propertyAlias, fieldName, errorMsg) {
            if (!propertyAlias) {
                return;
            }
            
            //only add the item if it doesn't exist                
            if (!this.hasPropertyError(propertyAlias, fieldName)) {
                this.items.push({
                    propertyAlias: propertyAlias,
                    fieldName: fieldName,
                    errorMsg: errorMsg
                });
            }
            
            //find all errors for this item
            var errorsForCallback = getPropertyErrors(this, propertyAlias, fieldName);
            //we should now call all of the call backs registered for this error
            var cbs = this.getPropertyCallbacks(propertyAlias, fieldName);
            //call each callback for this error
            for (var cb in cbs) {
                executeCallback(this, errorsForCallback, cbs[cb].callback);
            }
        },        
        
        /**
         * @ngdoc function
         * @name removePropertyError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Removes an error message for the content property
         */
        removePropertyError: function (propertyAlias, fieldName) {

            if (!propertyAlias) {
                return;
            }
            //remove the item
            this.items = _.reject(this.items, function (item) {
                return (item.propertyAlias === propertyAlias && (item.fieldName === fieldName || (fieldName === undefined || fieldName === "")));
            });
        },
        
        /**
         * @ngdoc function
         * @name reset
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Clears all errors and notifies all callbacks that all server errros are now valid - used when submitting a form
         */
        reset: function () {
            this.clear();
            for (var cb in callbacks) {
                callbacks[cb].callback.apply(this, [
                        true,       //pass in a value indicating it is VALID
                        [],         //pass in empty collection
                        []]);       //pass in empty collection
            }
        },
        
        /**
         * @ngdoc function
         * @name clear
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Clears all errors
         */
        clear: function() {
            this.items = [];
        },
        
        /**
         * @ngdoc function
         * @name getPropertyError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Gets the error message for the content property
         */
        getPropertyError: function (propertyAlias, fieldName) {
            var err = _.find(this.items, function (item) {
                //return true if the property alias matches and if an empty field name is specified or the field name matches
                return (item.propertyAlias === propertyAlias && (item.fieldName === fieldName || (fieldName === undefined || fieldName === "")));
            });
            return err;
        },
        
        /**
         * @ngdoc function
         * @name getFieldError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Gets the error message for a content field
         */
        getFieldError: function (fieldName) {
            var err = _.find(this.items, function (item) {
                //return true if the property alias matches and if an empty field name is specified or the field name matches
                return (item.propertyAlias === null && item.fieldName === fieldName);
            });
            return err;
        },
        
        /**
         * @ngdoc function
         * @name hasPropertyError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Checks if the content property + field name combo has an error
         */
        hasPropertyError: function (propertyAlias, fieldName) {
            var err = _.find(this.items, function (item) {
                //return true if the property alias matches and if an empty field name is specified or the field name matches
                return (item.propertyAlias === propertyAlias && (item.fieldName === fieldName || (fieldName === undefined || fieldName === "")));
            });
            return err ? true : false;
        },
        
        /**
         * @ngdoc function
         * @name hasFieldError
         * @methodOf umbraco.services.serverValidationManager
         * @function
         *
         * @description
         * Checks if a content field has an error
         */
        hasFieldError: function (fieldName) {
            var err = _.find(this.items, function (item) {
                //return true if the property alias matches and if an empty field name is specified or the field name matches
                return (item.propertyAlias === null && item.fieldName === fieldName);
            });
            return err ? true : false;
        },
        
        /** The array of error messages */
        items: []
    };
}

angular.module('umbraco.services').factory('serverValidationManager', serverValidationManager);
/**
 * @ngdoc service
 * @name umbraco.services.tinyMceService
 *
 *  
 * @description
 * A service containing all logic for all of the Umbraco TinyMCE plugins
 */
function tinyMceService(dialogService, $log, imageHelper, $http, $timeout, macroResource, macroService, $routeParams, umbRequestHelper) {
    return {

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#configuration
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Returns a collection of plugins available to the tinyMCE editor
        *
        */
        configuration: function () {
               return umbRequestHelper.resourcePromise(
                  $http.get(
                      umbRequestHelper.getApiUrl(
                          "rteApiBaseUrl",
                          "GetConfiguration")),
                  'Failed to retreive entity data for id '); 
        },

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#defaultPrevalues
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Returns a default configration to fallback on in case none is provided
        *
        */
        defaultPrevalues: function () {
               var cfg = {};
                       cfg.toolbar = ["code", "bold", "italic", "umbracocss","alignleft", "aligncenter", "alignright", "bullist","numlist", "outdent", "indent", "link", "image", "umbmediapicker", "umbembeddialog", "umbmacro"];
                       cfg.stylesheets = [];
                       cfg.dimensions = {height: 400, width: 600};
                return cfg;
        },

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#createInsertEmbeddedMedia
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Creates the umbrco insert embedded media tinymce plugin
        *
        * @param {Object} editor the TinyMCE editor instance        
        * @param {Object} $scope the current controller scope
        */
        createInsertEmbeddedMedia: function (editor, $scope) {
            editor.addButton('umbembeddialog', {
                icon: 'custom icon-tv',
                tooltip: 'Embed',
                onclick: function () {
                    dialogService.embedDialog({
                        scope: $scope, callback: function (data) {
                            editor.insertContent(data);
                        }
                    });
                }
            });
        },

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#createMediaPicker
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Creates the umbrco insert media tinymce plugin
        *
        * @param {Object} editor the TinyMCE editor instance        
        * @param {Object} $scope the current controller scope
        */
        createMediaPicker: function (editor, $scope) {
            editor.addButton('umbmediapicker', {
                icon: 'custom icon-picture',
                tooltip: 'Media Picker',
                onclick: function () {
                    dialogService.mediaPicker({
                        onlyImages: true,
                        scope: $scope, callback: function (img) {

                            if (img) {
                                var imagePropVal = imageHelper.getImagePropertyValue({ imageModel: img, scope: $scope });
                                var data = {
                                    alt: "Some description",
                                    src: (imagePropVal) ? imagePropVal : "nothing.jpg",
                                    id: '__mcenew'
                                };


                                editor.insertContent(editor.dom.createHTML('img', data));

                                $timeout(function () {
                                    var imgElm = editor.dom.get('__mcenew');
                                    var size = editor.dom.getSize(imgElm);

                                    var newSize = imageHelper.scaleToMaxSize(500, size.w, size.h);
                                    var s = "width: " + newSize.width + "px; height:" + newSize.height + "px;";
                                    editor.dom.setAttrib(imgElm, 'style', s);
                                    editor.dom.setAttrib(imgElm, 'rel', newSize.width + "," + newSize.height);
                                    editor.dom.setAttrib(imgElm, 'id', null);

                                }, 500);
                            }
                        }
                    });
                }
            });
        },

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#createLinkPicker
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Creates the umbrco insert link tinymce plugin
        *
        * @param {Object} editor the TinyMCE editor instance        
        * @param {Object} $scope the current controller scope
        */
        createLinkPicker: function (editor, $scope) {

            
            
            /*
            editor.addButton('link', {
                icon: 'custom icon-link',
                tooltip: 'Link Picker',
                onclick: function () {
                    dialogService.linkPicker({
                        scope: $scope, callback: function (link) {
                            if (link) {
                                var data = {
                                    title: "Some description",
                                    href: "",
                                    id: '__mcenew'
                                };

                                editor.execCommand("mceInsertLink", false, {
                                                href: "wat",
                                                title: "muh",
                                                target: null,
                                                "class": null
                                            });


                                //editor.insertContent(editor.dom.createHTML('a', data));
                           }
                        }
                    });
                }
            });*/
        },

        /**
        * @ngdoc method
        * @name umbraco.services.tinyMceService#createUmbracoMacro
        * @methodOf umbraco.services.tinyMceService
        *
        * @description
        * Creates the insert umbrco macro tinymce plugin
        *
        * @param {Object} editor the TinyMCE editor instance      
        * @param {Object} $scope the current controller scope
        */
        createInsertMacro: function (editor, $scope) {
            
            /** Adds custom rules for the macro plugin and custom serialization */
            editor.on('preInit', function (args) {
                //this is requires so that we tell the serializer that a 'div' is actually allowed in the root, otherwise the cleanup will strip it out
                editor.serializer.addRules('div');

                /** This checks if the div is a macro container, if so, checks if its wrapped in a p tag and then unwraps it (removes p tag) */
                editor.serializer.addNodeFilter('div', function (nodes, name) {
                    for (var i = 0; i < nodes.length; i++) {
                        if (nodes[i].attr("class") === "umb-macro-holder" && nodes[i].parent && nodes[i].parent.name.toUpperCase() === "P") {
                            nodes[i].parent.unwrap();
                        }
                    }
                });
            
            });

            /**
            * Because the macro gets wrapped in a P tag because of the way 'enter' works, this 
            * method will return the macro element if not wrapped in a p, or the p if the macro
            * element is the only one inside of it even if we are deep inside an element inside the macro
            */
            function getRealMacroElem(element) {
                var e = $(element).closest(".umb-macro-holder");
                if (e.length > 0) {
                    if (e.get(0).parentNode.nodeName === "P") {
                        //now check if we're the only element                    
                        if (element.parentNode.childNodes.length === 1) {
                            return e.get(0).parentNode;
                        }
                    }
                    return e.get(0);
                }
                return null;
            }

            /** loads in the macro content async from the server */
            function loadMacroContent($macroDiv, macroData) {
                
                //if we don't have the macroData, then we'll need to parse it from the macro div
                if (!macroData) {                    
                    var contents = $macroDiv.contents();
                    var comment = _.find(contents, function (item) {
                        return item.nodeType === 8;
                    });
                    if (!comment) {
                        throw "Cannot parse the current macro, the syntax in the editor is invalid";
                    }
                    var syntax = comment.textContent.trim();
                    var parsed = macroService.parseMacroSyntax(syntax);
                    macroData = parsed;
                }

                var $ins = $macroDiv.find("ins");

                //show the throbber
                $macroDiv.addClass("loading");

                var contentId = $routeParams.id;

                macroResource.getMacroResultAsHtmlForEditor(macroData.macroAlias, contentId, macroData.marcoParamsDictionary)
                    .then(function (htmlResult) {

                        $macroDiv.removeClass("loading");
                        htmlResult = htmlResult.trim();
                        if (htmlResult !== "") {
                            $ins.html(htmlResult);
                        }
                    });
            }
            
            /** Adds the button instance */
            editor.addButton('umbmacro', {
                icon: 'custom icon-settings-alt',
                tooltip: 'Insert macro',
                onPostRender: function () {

                    var ctrl = this;
                    var isOnMacroElement = false;

                    /**
                     if the selection comes from a different element that is not the macro's
                     we need to check if the selection includes part of the macro, if so we'll force the selection
                     to clear to the next element since if people can select part of the macro markup they can then modify it.
                    */
                    function handleSelectionChange() {

                        if (!editor.selection.isCollapsed()) {
                            var endSelection = tinymce.activeEditor.selection.getEnd();
                            var startSelection = tinymce.activeEditor.selection.getStart();
                            //don't proceed if it's an entire element selected
                            if (endSelection !== startSelection) { 
                                
                                //if the end selection is a macro then move the cursor
                                //NOTE: we don't have to handle when the selection comes from a previous parent because
                                // that is automatically taken care of with the normal onNodeChanged logic since the 
                                // evt.element will be the macro once it becomes part of the selection.
                                var $testForMacro = $(endSelection).closest(".umb-macro-holder");
                                if ($testForMacro.length > 0) {
                                    
                                    //it came from before so move after, if there is no after then select ourselves
                                    var next = $testForMacro.next();
                                    if (next.length > 0) {
                                        editor.selection.setCursorLocation($testForMacro.next().get(0));
                                    }
                                    else {
                                        selectMacroElement($testForMacro.get(0));
                                    }

                                }
                            }
                        }
                    }

                    /** helper method to select the macro element */
                    function selectMacroElement(macroElement) {
                        // move selection to top element to ensure we can't edit this
                        editor.selection.select(macroElement);

                        // check if the current selection *is* the element (ie bug)
                        var currentSelection = editor.selection.getStart();
                        if (tinymce.isIE) {
                            if (!editor.dom.hasClass(currentSelection, 'umb-macro-holder')) {
                                while (!editor.dom.hasClass(currentSelection, 'umb-macro-holder') && currentSelection.parentNode) {
                                    currentSelection = currentSelection.parentNode;
                                }
                                editor.selection.select(currentSelection);
                            }
                        }
                    }

                    /**
                    * Add a node change handler, test if we're editing a macro and select the whole thing, then set our isOnMacroElement flag.
                    * If we change the selection inside this method, then we end up in an infinite loop, so we have to remove ourselves
                    * from the event listener before changing selection, however, it seems that putting a break point in this method
                    * will always cause an 'infinite' loop as the caret keeps changing.
                    */
                    function onNodeChanged(evt) {

                        //set our macro button active when on a node of class umb-macro-holder
                        var $macroElement = $(evt.element).closest(".umb-macro-holder");
                        
                        handleSelectionChange();

                        //set the button active
                        ctrl.active($macroElement.length !== 0);

                        if ($macroElement.length > 0) {
                            var macroElement = $macroElement.get(0);

                            //remove the event listener before re-selecting
                            editor.off('NodeChange', onNodeChanged);

                            selectMacroElement(macroElement);

                            //set the flag
                            isOnMacroElement = true;

                            //re-add the event listener
                            editor.on('NodeChange', onNodeChanged);
                        }
                        else {
                            isOnMacroElement = false;
                        }

                    }

                    /** when the contents load we need to find any macros declared and load in their content */
                    editor.on("LoadContent", function (o) {
                        
                        //get all macro divs and load their content
                        $(editor.dom.select(".umb-macro-holder.mceNonEditable")).each(function() {
                            loadMacroContent($(this));
                        });                        

                    });

                    /** This prevents any other commands from executing when the current element is the macro so the content cannot be edited */
                    editor.on('BeforeExecCommand', function (o) {                        
                        if (isOnMacroElement) {
                            if (o.preventDefault) {
                                o.preventDefault();
                            }
                            if (o.stopImmediatePropagation) {
                                o.stopImmediatePropagation();
                            }
                            return;
                        }
                    });
                    
                    /** This double checks and ensures you can't paste content into the rendered macro */
                    editor.on("Paste", function (o) {                        
                        if (isOnMacroElement) {
                            if (o.preventDefault) {
                                o.preventDefault();
                            }
                            if (o.stopImmediatePropagation) {
                                o.stopImmediatePropagation();
                            }
                            return;
                        }
                    });

                    //set onNodeChanged event listener
                    editor.on('NodeChange', onNodeChanged);

                    /** 
                    * Listen for the keydown in the editor, we'll check if we are currently on a macro element, if so
                    * we'll check if the key down is a supported key which requires an action, otherwise we ignore the request
                    * so the macro cannot be edited.
                    */
                    editor.on('KeyDown', function (e) {
                        if (isOnMacroElement) {
                            var macroElement = editor.selection.getNode();

                            //get the 'real' element (either p or the real one)
                            macroElement = getRealMacroElem(macroElement);

                            //prevent editing
                            e.preventDefault();
                            e.stopPropagation();

                            var moveSibling = function (element, isNext) {
                                var $e = $(element);
                                var $sibling = isNext ? $e.next() : $e.prev();
                                if ($sibling.length > 0) {
                                    editor.selection.select($sibling.get(0));
                                    editor.selection.collapse(true);
                                }
                                else {
                                    //if we're moving previous and there is no sibling, then lets recurse and just select the next one
                                    if (!isNext) {
                                        moveSibling(element, true);
                                        return;
                                    }

                                    //if there is no sibling we'll generate a new p at the end and select it
                                    editor.setContent(editor.getContent() + "<p>&nbsp;</p>");
                                    editor.selection.select($(editor.dom.getRoot()).children().last().get(0));
                                    editor.selection.collapse(true);

                                }
                            };

                            //supported keys to move to the next or prev element (13-enter, 27-esc, 38-up, 40-down, 39-right, 37-left)
                            //supported keys to remove the macro (8-backspace, 46-delete)
                            //TODO: Should we make the enter key insert a line break before or leave it as moving to the next element?
                            if ($.inArray(e.keyCode, [13, 40, 39]) !== -1) {
                                //move to next element
                                moveSibling(macroElement, true);
                            }
                            else if ($.inArray(e.keyCode, [27, 38, 37]) !== -1) {
                                //move to prev element
                                moveSibling(macroElement, false);
                            }
                            else if ($.inArray(e.keyCode, [8, 46]) !== -1) {
                                //delete macro element

                                //move first, then delete
                                moveSibling(macroElement, false);
                                editor.dom.remove(macroElement);
                            }
                            return ;
                        }
                    });

                },
                
                /** The insert macro button click event handler */
                onclick: function () {

                    var dialogData = {
                        //flag for use in rte so we only show macros flagged for the editor
                        richTextEditor: true  
                    };

                    //when we click we could have a macro already selected and in that case we'll want to edit the current parameters
                    //so we'll need to extract them and submit them to the dialog.
                    var macroElement = editor.selection.getNode();                    
                    macroElement = getRealMacroElem(macroElement);
                    if (macroElement) {
                        //we have a macro selected so we'll need to parse it's alias and parameters
                        var contents = $(macroElement).contents();                        
                        var comment = _.find(contents, function(item) {
                            return item.nodeType === 8;
                        });
                        if (!comment) {
                            throw "Cannot parse the current macro, the syntax in the editor is invalid";
                        }
                        var syntax = comment.textContent.trim();
                        var parsed = macroService.parseMacroSyntax(syntax);
                        dialogData = {
                            macroData: parsed  
                        };
                    }

                    dialogService.macroPicker({
                        scope: $scope,
                        dialogData : dialogData,
                        callback: function(data) {

                            //put the macro syntax in comments, we will parse this out on the server side to be used
                            //for persisting.
                            var macroSyntaxComment = "<!-- " + data.syntax + " -->";
                            //create an id class for this element so we can re-select it after inserting
                            var uniqueId = "umb-macro-" + editor.dom.uniqueId();
                            var macroDiv = editor.dom.create('div',
                                {
                                    'class': 'umb-macro-holder ' + data.macroAlias + ' mceNonEditable ' + uniqueId
                                },
                                macroSyntaxComment + '<ins>Macro alias: <strong>' + data.macroAlias + '</strong></ins>');

                            editor.selection.setNode(macroDiv);
                            
                            var $macroDiv = $(editor.dom.select("div.umb-macro-holder." + uniqueId));

                            //async load the macro content
                            loadMacroContent($macroDiv, data);                          
                        }
                    });

                }
            });
        }
    };
}

angular.module('umbraco.services').factory('tinyMceService', tinyMceService);

/**
 * @ngdoc service
 * @name umbraco.services.treeService
 * @function
 *
 * @description
 * The tree service factory, used internally by the umbTree and umbTreeItem directives
 */
function treeService($q, treeResource, iconHelper, notificationsService, $rootScope) {

    //SD: Have looked at putting this in sessionStorage (not localStorage since that means you wouldn't be able to work
    // in multiple tabs) - however our tree structure is cyclical, meaning a node has a reference to it's parent and it's children
    // which you cannot serialize to sessionStorage. There's really no benefit of session storage except that you could refresh
    // a tab and have the trees where they used to be - supposed that is kind of nice but would mean we'd have to store the parent
    // as a nodeid reference instead of a variable with a getParent() method.
    var treeCache = {};
    
    var standardCssClass = 'icon umb-tree-icon sprTree';

    function getCacheKey(args) {
        //if there is no cache key they return null - it won't be cached.
        if (!args || !args.cacheKey) {
            return null;
        }        

        var cacheKey = args.cacheKey;
        cacheKey += "_" + args.section;
        return cacheKey;
    }

    return {  

        /** Internal method to return the tree cache */
        _getTreeCache: function() {
            return treeCache;
        },

        /** Internal method that ensures there's a routePath, parent and level property on each tree node and adds some icon specific properties so that the nodes display properly */
        _formatNodeDataForUseInUI: function (parentNode, treeNodes, section, level) {
            //if no level is set, then we make it 1   
            var childLevel = (level ? level : 1);
            //set the section if it's not already set
            if (!parentNode.section) {
                parentNode.section = section;
            }
            //create a method outside of the loop to return the parent - otherwise jshint blows up
            var funcParent = function() {
                return parentNode;
            };
            for (var i = 0; i < treeNodes.length; i++) {

                treeNodes[i].level = childLevel;

                //create a function to get the parent node, we could assign the parent node but 
                // then we cannot serialize this entity because we have a cyclical reference.
                // Instead we just make a function to return the parentNode.
                treeNodes[i].parent = funcParent;

                //set the section for each tree node - this allows us to reference this easily when accessing tree nodes
                treeNodes[i].section = section;

                //if there is not route path specified, then set it automatically,
                //if this is a tree root node then we want to route to the section's dashboard
                if (!treeNodes[i].routePath) {
                    
                    if (treeNodes[i].metaData && treeNodes[i].metaData["treeAlias"]) {
                        //this is a root node
                        treeNodes[i].routePath = section;                        
                    }
                    else {
                        var treeAlias = this.getTreeAlias(treeNodes[i]);
                        treeNodes[i].routePath = section + "/" + treeAlias + "/edit/" + treeNodes[i].id;
                    }
                }

                //now, format the icon data
                if (treeNodes[i].iconIsClass === undefined || treeNodes[i].iconIsClass) {
                    var converted = iconHelper.convertFromLegacyTreeNodeIcon(treeNodes[i]);
                    treeNodes[i].cssClass = standardCssClass + " " + converted;
                    if (converted.startsWith('.')) {
                        //its legacy so add some width/height
                        treeNodes[i].style = "height:16px;width:16px;";
                    }
                    else {
                        treeNodes[i].style = "";
                    }
                }
                else {
                    treeNodes[i].style = "background-image: url('" + treeNodes[i].iconFilePath + "');";
                    //we need an 'icon-' class in there for certain styles to work so if it is image based we'll add this
                    treeNodes[i].cssClass = standardCssClass + " legacy-custom-file";
                }
            }
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getTreePackageFolder
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Determines if the current tree is a plugin tree and if so returns the package folder it has declared
         * so we know where to find it's views, otherwise it will just return undefined.
         * 
         * @param {String} treeAlias The tree alias to check
         */
        getTreePackageFolder: function(treeAlias) {            
            //we determine this based on the server variables
            if (Umbraco.Sys.ServerVariables.umbracoPlugins &&
                Umbraco.Sys.ServerVariables.umbracoPlugins.trees &&
                angular.isArray(Umbraco.Sys.ServerVariables.umbracoPlugins.trees)) {

                var found = _.find(Umbraco.Sys.ServerVariables.umbracoPlugins.trees, function(item) {
                    return item.alias === treeAlias;
                });
                
                return found ? found.packageFolder : undefined;
            }
            return undefined;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#clearCache
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Clears the tree cache - with optional cacheKey, optional section or optional filter.
         * 
         * @param {Object} args arguments
         * @param {String} args.cacheKey optional cachekey - this is used to clear specific trees in dialogs
         * @param {String} args.section optional section alias - clear tree for a given section
         * @param {String} args.childrenOf optional parent ID - only clear the cache below a specific node
         */
        clearCache: function (args) {
            //clear all if not specified
            if (!args) {
                treeCache = {};
            }
            else {
                //if section and cache key specified just clear that cache
                if (args.section && args.cacheKey) {
                    var cacheKey = getCacheKey(args);
                    if (cacheKey && treeCache && treeCache[cacheKey] != null) {
                        treeCache = _.omit(treeCache, cacheKey);
                    }
                }
                else if (args.childrenOf) {
                    //if childrenOf is supplied a cacheKey must be supplied as well
                    if (!args.cacheKey) {
                        throw "args.cacheKey is required if args.childrenOf is supplied";
                    }
                    //this will clear out all children for the parentId passed in to this parameter, we'll 
                    // do this by recursing and specifying a filter
                    var self = this;
                    this.clearCache({
                        cacheKey: args.cacheKey,
                        filter: function(cc) {
                            //get the new parent node from the tree cache
                            var parent = self.getDescendantNode(cc.root, args.childrenOf);
                            if (parent) {
                                //clear it's children and set to not expanded
                                parent.children = null;
                                parent.expanded = false;
                            }
                            //return the cache to be saved
                            return cc;
                        }
                    });
                }
                else if (args.filter && angular.isFunction(args.filter)) {
                    //if a filter is supplied a cacheKey must be supplied as well
                    if (!args.cacheKey) {
                        throw "args.cacheKey is required if args.filter is supplied";
                    }

                    //if a filter is supplied the function needs to return the data to keep
                    var byKey = treeCache[args.cacheKey];
                    if (byKey) {
                        var result = args.filter(byKey);

                        if (result) {
                            //set the result to the filtered data
                            treeCache[args.cacheKey] = result;
                        }
                        else {                            
                            //remove the cache
                            treeCache = _.omit(treeCache, args.cacheKey);
                        }

                    }

                }
                else if (args.cacheKey) {
                    //if only the cache key is specified, then clear all cache starting with that key
                    var allKeys1 = _.keys(treeCache);
                    var toRemove1 = _.filter(allKeys1, function (k) {
                        return k.startsWith(args.cacheKey + "_");
                    });
                    treeCache = _.omit(treeCache, toRemove1);
                }
                else if (args.section) {
                    //if only the section is specified then clear all cache regardless of cache key by that section
                    var allKeys2 = _.keys(treeCache);
                    var toRemove2 = _.filter(allKeys2, function (k) {
                        return k.endsWith("_" + args.section);
                    });
                    treeCache = _.omit(treeCache, toRemove2);
                }               
            }
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#loadNodeChildren
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Clears all node children, gets it's up-to-date children from the server and re-assigns them and then
         * returns them in a promise.
         * @param {object} args An arguments object
         * @param {object} args.node The tree node
         * @param {object} args.section The current section
         */
        loadNodeChildren: function(args) {
            if (!args) {
                throw "No args object defined for loadNodeChildren";
            }
            if (!args.node) {
                throw "No node defined on args object for loadNodeChildren";
            }
            
            this.removeChildNodes(args.node);
            args.node.loading = true;

            return this.getChildren(args)
                .then(function(data) {

                    //set state to done and expand (only if there actually are children!)
                    args.node.loading = false;
                    args.node.children = data;
                    if (args.node.children && args.node.children.length > 0) {
                        args.node.expanded = true;
                        args.node.hasChildren = true;
                    }
                    return data;

                }, function(reason) {

                    //in case of error, emit event
                    $rootScope.$broadcast("treeNodeLoadError", {error: reason });

                    //stop show the loading indicator  
                    node.loading = false;

                    //tell notications about the error
                    notificationsService.error(reason);

                    return reason;
                });

        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#removeNode
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Removes a given node from the tree
         * @param {object} treeNode the node to remove
         */
        removeNode: function(treeNode) {
            if (treeNode.parent() == null) {
                throw "Cannot remove a node that doesn't have a parent";
            }
            //remove the current item from it's siblings
            treeNode.parent().children.splice(treeNode.parent().children.indexOf(treeNode), 1);            
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.treeService#removeChildNodes
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Removes all child nodes from a given tree node 
         * @param {object} treeNode the node to remove children from
         */
        removeChildNodes : function(treeNode) {
            treeNode.expanded = false;
            treeNode.children = [];
            treeNode.hasChildren = false;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getChildNode
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Gets a child node with a given ID, from a specific treeNode
         * @param {object} treeNode to retrive child node from
         * @param {int} id id of child node
         */
        getChildNode: function (treeNode, id) {
            if (!treeNode.children) {
                return null;
            }
            var found = _.find(treeNode.children, function (child) {
                return String(child.id) === String(id);
            });
            return found === undefined ? null : found;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getDescendantNode
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Gets a descendant node by id
         * @param {object} treeNode to retrive descendant node from
         * @param {int} id id of descendant node
         * @param {string} treeAlias - optional tree alias, if fetching descendant node from a child of a listview document
         */
        getDescendantNode: function(treeNode, id, treeAlias) {

            //validate if it is a section container since we'll need a treeAlias if it is one
            if (treeNode.isContainer === true && !treeAlias) {
                throw "Cannot get a descendant node from a section container node without a treeAlias specified";
            }

            //if it is a section container, we need to find the tree to be searched
            if (treeNode.isContainer) {
                var foundRoot = null;
                for (var c = 0; c < treeNode.children.length; c++) {
                    if (this.getTreeAlias(treeNode.children[c]) === treeAlias) {
                        foundRoot = treeNode.children[c];
                        break;
                    }
                }
                if (!foundRoot) {
                    throw "Could not find a tree in the current section with alias " + treeAlias;
                }
                treeNode = foundRoot;
            }

            //check this node
            if (treeNode.id === id) {
                return treeNode;
            }

            //check the first level
            var found = this.getChildNode(treeNode, id);
            if (found) {
                return found;
            }
           
            //check each child of this node
            if (!treeNode.children) {
                return null;
            }

            for (var i = 0; i < treeNode.children.length; i++) {
                if (treeNode.children[i].children && angular.isArray(treeNode.children[i].children) && treeNode.children[i].children.length > 0) {
                    //recurse
                    found = this.getDescendantNode(treeNode.children[i], id);
                    if (found) {
                        return found;
                    }
                }
            }
            
            //not found
            return found === undefined ? null : found;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getTreeRoot
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Gets the root node of the current tree type for a given tree node
         * @param {object} treeNode to retrive tree root node from
         */
        getTreeRoot: function (treeNode) {
            if (!treeNode) {
                throw "treeNode cannot be null";
            }

            //all root nodes have metadata key 'treeAlias'
            var root = null;
            var current = treeNode;            
            while (root === null && current) {
                
                if (current.metaData && current.metaData["treeAlias"]) {
                    root = current;
                }
                else { 
                    current = current.parent();
                }
            }
            return root;
        },

        /** Gets the node's tree alias, this is done by looking up the meta-data of the current node's root node */
        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getTreeAlias
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Gets the node's tree alias, this is done by looking up the meta-data of the current node's root node 
         * @param {object} treeNode to retrive tree alias from
         */
        getTreeAlias : function(treeNode) {
            var root = this.getTreeRoot(treeNode);
            if (root) {
                return root.metaData["treeAlias"];
            }
            return "";
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getTree
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * gets the tree, returns a promise 
         * @param {object} args Arguments
         * @param {string} args.section Section alias
         * @param {string} args.cacheKey Optional cachekey
         */
        getTree: function (args) {

            var deferred = $q.defer();

            //set defaults
            if (!args) {
                args = { section: 'content', cacheKey: null };
            }
            else if (!args.section) {
                args.section = 'content';
            }

            var cacheKey = getCacheKey(args);
            
            //return the cache if it exists
            if (cacheKey && treeCache[cacheKey] !== undefined) {
                deferred.resolve(treeCache[cacheKey]);
                return deferred.promise;
            }

            var self = this;
            treeResource.loadApplication(args)
                .then(function(data) {
                    //this will be called once the tree app data has loaded
                    var result = {
                        name: data.name,
                        alias: args.section,
                        root: data
                    };
                    //we need to format/modify some of the node data to be used in our app.
                    self._formatNodeDataForUseInUI(result.root, result.root.children, args.section);

                    //cache this result if a cache key is specified - generally a cache key should ONLY
                    // be specified for application trees, dialog trees should not be cached.
                    if (cacheKey) {                        
                        treeCache[cacheKey] = result;
                        deferred.resolve(treeCache[cacheKey]);
                    }

                    //return un-cached
                    deferred.resolve(result);
                });
            
            return deferred.promise;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getMenu
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Returns available menu actions for a given tree node
         * @param {object} args Arguments
         * @param {string} args.treeNode tree node object to retrieve the menu for
         */
        getMenu: function (args) {

            if (!args) {
                throw "args cannot be null";
            }
            if (!args.treeNode) {
                throw "args.treeNode cannot be null";
            }

            return treeResource.loadMenu(args.treeNode)
                .then(function(data) {
                    //need to convert the icons to new ones
                    for (var i = 0; i < data.length; i++) {
                        data[i].cssclass = iconHelper.convertFromLegacyIcon(data[i].cssclass);
                    }
                    return data;
                });
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getChildren
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Gets the children from the server for a given node 
         * @param {object} args Arguments
         * @param {object} args.node tree node object to retrieve the children for
         * @param {string} args.section current section alias
         */
        getChildren: function (args) {

            if (!args) {
                throw "No args object defined for getChildren";
            }
            if (!args.node) {
                throw "No node defined on args object for getChildren";
            }

            var section = args.section || 'content';
            var treeItem = args.node;

            var self = this;

            return treeResource.loadNodes({ node: treeItem })
                .then(function (data) {
                    //now that we have the data, we need to add the level property to each item and the view
                    self._formatNodeDataForUseInUI(treeItem, data, section, treeItem.level + 1);
                    return data;
                });
        },
        
        /**
         * @ngdoc method
         * @name umbraco.services.treeService#reloadNode
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * Re-loads the single node from the server
         * @param {object} node Tree node to reload
         */
        reloadNode: function(node) {
            if (!node) {
                throw "node cannot be null";
            }
            if (!node.parent()) {
                throw "cannot reload a single node without a parent";
            }
            if (!node.section) {
                throw "cannot reload a single node without an assigned node.section";
            }
            
            var deferred = $q.defer();
            
            //set the node to loading
            node.loading = true;

            this.getChildren({ node: node.parent(), section: node.section }).then(function(data) {

                //ok, now that we have the children, find the node we're reloading
                var found = _.find(data, function(item) {
                    return item.id === node.id;
                });
                if (found) {
                    //now we need to find the node in the parent.children collection to replace
                    var index = _.indexOf(node.parent().children, node);
                    //the trick here is to not actually replace the node - this would cause the delete animations
                    //to fire, instead we're just going to replace all the properties of this node.

                    //there should always be a method assigned but we'll check anyways
                    if (angular.isFunction(node.parent().children[index].updateNodeData)) {
                        node.parent().children[index].updateNodeData(found);
                    }
                    else {
                        //just update as per normal - this means styles, etc.. won't be applied
                        _.extend(node.parent().children[index], found);
                    }
                    
                    //set the node loading
                    node.parent().children[index].loading = false;
                    //return
                    deferred.resolve(node.parent().children[index]);
                }
                else {
                    deferred.reject();
                }
            }, function() {
                deferred.reject();
            });
            
            return deferred.promise;
        },

        /**
         * @ngdoc method
         * @name umbraco.services.treeService#getPath
         * @methodOf umbraco.services.treeService
         * @function
         *
         * @description
         * This will return the current node's path by walking up the tree 
         * @param {object} node Tree node to retrieve path for
         */
        getPath: function(node) {
            if (!node) {
                throw "node cannot be null";                
            }
            if (!angular.isFunction(node.parent)) {
                throw "node.parent is not a function, the path cannot be resolved";
            }
            //all root nodes have metadata key 'treeAlias'
            var reversePath = [];
            var current = node;
            while (current != null) {
                reversePath.push(current.id);                
                if (current.metaData && current.metaData["treeAlias"]) {
                    current = null;
                }
                else {
                    current = current.parent();
                }
            }
            return reversePath.reverse();
        },

        syncTree: function(args) {
            
            if (!args) {
                throw "No args object defined for syncTree";
            }
            if (!args.node) {
                throw "No node defined on args object for syncTree";
            }
            if (!args.path) {
                throw "No path defined on args object for syncTree";
            }
            if (!angular.isArray(args.path)) {
                throw "Path must be an array";
            }
            if (args.path.length < 1) {
                throw "args.path must contain at least one id";
            }

            var deferred = $q.defer();

            //get the rootNode for the current node, we'll sync based on that
            var root = this.getTreeRoot(args.node);
            if (!root) {
                throw "Could not get the root tree node based on the node passed in";
            }
            
            //now we want to loop through the ids in the path, first we'll check if the first part
            //of the path is the root node, otherwise we'll search it's children.
            var currPathIndex = 0;
            //if the first id is the root node and there's only one... then consider it synced
            if (String(args.path[currPathIndex]) === String(args.node.id)) {
                if (args.path.length === 1) {
                    //return the root
                    deferred.resolve(root);
                    return deferred.promise;
                }
                else {
                    //move to the next path part and continue
                    currPathIndex = 1;
                }
            }
           
            //now that we have the first id to lookup, we can start the process

            var self = this;
            var node = args.node;

            var doSync = function () {
                //check if it exists in the already loaded children
                var child = self.getChildNode(node, args.path[currPathIndex]);
                if (child) {
                    if (args.path.length === (currPathIndex + 1)) {
                        //woot! synced the node
                        if (!args.forceReload) {
                            deferred.resolve(child);
                        }
                        else {
                            //even though we've found the node if forceReload is specified
                            //we want to go update this single node from the server
                            self.reloadNode(child).then(function (reloaded) {
                                deferred.resolve(reloaded);
                            }, function () {
                                deferred.reject();
                            });
                        }
                    }
                    else {
                        //now we need to recurse with the updated node/currPathIndex
                        currPathIndex++;
                        node = child;
                        //recurse
                        doSync();
                    }
                }
                else {
                    //couldn't find it in the 
                    self.loadNodeChildren({ node: node, section: node.section }).then(function () {
                        //ok, got the children, let's find it
                        var found = self.getChildNode(node, args.path[currPathIndex]);
                        if (found) {
                            if (args.path.length === (currPathIndex + 1)) {
                                //woot! synced the node
                                deferred.resolve(found);
                            }
                            else {
                                //now we need to recurse with the updated node/currPathIndex
                                currPathIndex++;
                                node = found;
                                //recurse
                                doSync();
                            }
                        }
                        else {
                            //fail!
                            deferred.reject();
                        }
                    }, function () {
                        //fail!
                        deferred.reject();
                    });
                }
            };

            //start
            doSync();
            
            return deferred.promise;

        }
        
    };
}

angular.module('umbraco.services').factory('treeService', treeService);
/**
* @ngdoc service
* @name umbraco.services.umbRequestHelper
* @description A helper object used for sending requests to the server
**/
function umbRequestHelper($http, $q, umbDataFormatter, angularHelper, dialogService, notificationsService) {
    return {

        /**
         * @ngdoc method
         * @name umbraco.services.umbRequestHelper#dictionaryToQueryString
         * @methodOf umbraco.services.umbRequestHelper
         * @function
         *
         * @description
         * This will turn an array of key/value pairs into a query string
         * 
         * @param {Array} queryStrings An array of key/value pairs
         */
        dictionaryToQueryString: function (queryStrings) {

            
            if (angular.isArray(queryStrings)) {
                return _.map(queryStrings, function (item) {
                    var key = null;
                    var val = null;
                    for (var k in item) {
                        key = k;
                        val = item[k];
                        break;
                    }
                    if (key === null || val === null) {
                        throw "The object in the array was not formatted as a key/value pair";
                    }
                    return encodeURIComponent(key) + "=" + encodeURIComponent(val);
                }).join("&");
            }

            /*
            //if we have a simple object, we can simply map with $.param
            //but with the current structure we cant since an array is an object and an object is an array
            if(angular.isObject(queryStrings)){
                return decodeURIComponent($.param(queryStrings)); 
            }*/

            throw "The queryString parameter is not an array of key value pairs";
        },

        /**
         * @ngdoc method
         * @name umbraco.services.umbRequestHelper#getApiUrl
         * @methodOf umbraco.services.umbRequestHelper
         * @function
         *
         * @description
         * This will return the webapi Url for the requested key based on the servervariables collection
         * 
         * @param {string} apiName The webapi name that is found in the servervariables["umbracoUrls"] dictionary
         * @param {string} actionName The webapi action name 
         * @param {object} queryStrings Can be either a string or an array containing key/value pairs
         */
        getApiUrl: function (apiName, actionName, queryStrings) {
            if (!Umbraco || !Umbraco.Sys || !Umbraco.Sys.ServerVariables || !Umbraco.Sys.ServerVariables["umbracoUrls"]) {
                throw "No server variables defined!";
            }

            if (!Umbraco.Sys.ServerVariables["umbracoUrls"][apiName]) {
                throw "No url found for api name " + apiName;
            }

            return Umbraco.Sys.ServerVariables["umbracoUrls"][apiName] + actionName +
                (!queryStrings ? "" : "?" + (angular.isString(queryStrings) ? queryStrings : this.dictionaryToQueryString(queryStrings)));

        },

        /**
         * @ngdoc function
         * @name umbraco.services.umbRequestHelper#resourcePromise
         * @methodOf umbraco.services.umbRequestHelper
         * @function
         *
         * @description
         * This returns a promise with an underlying http call, it is a helper method to reduce
         *  the amount of duplicate code needed to query http resources and automatically handle any 
         *  Http errors. See /docs/source/using-promises-resources.md
         *
         * @param {object} opts A mixed object which can either be a string representing the error message to be
         *   returned OR an object containing either:
         *     { success: successCallback, errorMsg: errorMessage }
         *          OR
         *     { success: successCallback, error: errorCallback }
         *   In both of the above, the successCallback must accept these parameters: data, status, headers, config
         *   If using the errorCallback it must accept these parameters: data, status, headers, config
         *   The success callback must return the data which will be resolved by the deferred object.
         *   The error callback must return an object containing: {errorMsg: errorMessage, data: originalData, status: status }
         */
        resourcePromise: function (httpPromise, opts) {
            var deferred = $q.defer();

            /** The default success callback used if one is not supplied in the opts */
            function defaultSuccess(data, status, headers, config) {
                //when it's successful, just return the data
                return data;
            }

            /** The default error callback used if one is not supplied in the opts */
            function defaultError(data, status, headers, config) {
                return {
                    //NOTE: the default error message here should never be used based on the above docs!
                    errorMsg: (angular.isString(opts) ? opts : 'An error occurred!'),
                    data: data,
                    status: status
                };
            }

            //create the callbacs based on whats been passed in.
            var callbacks = {
                success: ((!opts || !opts.success) ? defaultSuccess : opts.success),
                error: ((!opts || !opts.error) ? defaultError : opts.error)
            };

            httpPromise.success(function (data, status, headers, config) {

                //invoke the callback 
                var result = callbacks.success.apply(this, [data, status, headers, config]);

                //when it's successful, just return the data
                deferred.resolve(result);

            }).error(function (data, status, headers, config) {

                //invoke the callback
                var result = callbacks.error.apply(this, [data, status, headers, config]);

                //when there's a 500 (unhandled) error show a YSOD overlay if debugging is enabled.
                if (status >= 500 && status < 600) {

                    //show a ysod dialog
                    if (Umbraco.Sys.ServerVariables["isDebuggingEnabled"] === true) {
                        dialogService.ysodDialog({
                            errorMsg: result.errorMsg,
                            data: result.data
                        });
                    }
                    else {
                        //show a simple error notification                         
                        notificationsService.error("Server error", "Contact administrator, see log for full details.<br/><i>" + result.errorMsg + "</i>");
                    }
                    
                }
                else {

                    //return an error object including the error message for UI
                    deferred.reject({
                        errorMsg: result.errorMsg,
                        data: result.data,
                        status: result.status
                    });

                }

            });

            return deferred.promise;

        },

        /** Used for saving media/content specifically */
        postSaveContent: function (args) {

            if (!args.restApiUrl) {
                throw "args.restApiUrl is a required argument";
            }
            if (!args.content) {
                throw "args.content is a required argument";
            }
            if (!args.action) {
                throw "args.action is a required argument";
            }
            if (!args.files) {
                throw "args.files is a required argument";
            }
            if (!args.dataFormatter) {
                throw "args.dataFormatter is a required argument";
            }


            var deferred = $q.defer();

            //save the active tab id so we can set it when the data is returned.
            var activeTab = _.find(args.content.tabs, function (item) {
                return item.active;
            });
            var activeTabIndex = (activeTab === undefined ? 0 : _.indexOf(args.content.tabs, activeTab));

            //save the data
            this.postMultiPartRequest(
                args.restApiUrl,
                { key: "contentItem", value: args.dataFormatter(args.content, args.action) },
                function (data, formData) {
                    //now add all of the assigned files
                    for (var f in args.files) {
                        //each item has a property id and the file object, we'll ensure that the id is suffixed to the key
                        // so we know which property it belongs to on the server side
                        formData.append("file_" + args.files[f].id, args.files[f].file);
                    }

                },
                function (data, status, headers, config) {
                    //success callback

                    //reset the tabs and set the active one
                    _.each(data.tabs, function (item) {
                        item.active = false;
                    });
                    data.tabs[activeTabIndex].active = true;

                    //the data returned is the up-to-date data so the UI will refresh
                    deferred.resolve(data);
                },
                function (data, status, headers, config) {
                    //failure callback

                    //when there's a 500 (unhandled) error show a YSOD overlay if debugging is enabled.
                    if (status >= 500 && status < 600) {

                        //show a ysod dialog
                        if (Umbraco.Sys.ServerVariables["isDebuggingEnabled"] === true) {
                            dialogService.ysodDialog({
                                errorMsg: 'An error occurred',
                                data: data
                            });
                        }
                        else {
                            //show a simple error notification                         
                            notificationsService.error("Server error", "Contact administrator, see log for full details.<br/><i>" + data.ExceptionMessage + "</i>");
                        }
                        
                    }
                    else {

                        //return an error object including the error message for UI
                        deferred.reject({
                            errorMsg: 'An error occurred',
                            data: data,
                            status: status
                        });
                    }

                });

            return deferred.promise;
        },

        /** Posts a multi-part mime request to the server */
        postMultiPartRequest: function (url, jsonData, transformCallback, successCallback, failureCallback) {

            //validate input, jsonData can be an array of key/value pairs or just one key/value pair.
            if (!jsonData) { throw "jsonData cannot be null"; }

            if (angular.isArray(jsonData)) {
                _.each(jsonData, function (item) {
                    if (!item.key || !item.value) { throw "jsonData array item must have both a key and a value property"; }
                });
            }
            else if (!jsonData.key || !jsonData.value) { throw "jsonData object must have both a key and a value property"; }


            $http({
                method: 'POST',
                url: url,
                //IMPORTANT!!! You might think this should be set to 'multipart/form-data' but this is not true because when we are sending up files
                // the request needs to include a 'boundary' parameter which identifies the boundary name between parts in this multi-part request
                // and setting the Content-type manually will not set this boundary parameter. For whatever reason, setting the Content-type to 'false'
                // will force the request to automatically populate the headers properly including the boundary parameter.
                headers: { 'Content-Type': false },
                transformRequest: function (data) {
                    var formData = new FormData();
                    //add the json data
                    if (angular.isArray(data)) {
                        _.each(data, function (item) {
                            formData.append(item.key, !angular.isString(item.value) ? angular.toJson(item.value) : item.value);
                        });
                    }
                    else {
                        formData.append(data.key, !angular.isString(data.value) ? angular.toJson(data.value) : data.value);
                    }

                    //call the callback
                    if (transformCallback) {
                        transformCallback.apply(this, [data, formData]);
                    }

                    return formData;
                },
                data: jsonData
            }).
            success(function (data, status, headers, config) {
                if (successCallback) {
                    successCallback.apply(this, [data, status, headers, config]);
                }
            }).
            error(function (data, status, headers, config) {
                if (failureCallback) {
                    failureCallback.apply(this, [data, status, headers, config]);
                }
            });
        }
    };
}
angular.module('umbraco.services').factory('umbRequestHelper', umbRequestHelper);
angular.module('umbraco.services')
.factory('userService', function ($rootScope, $q, $location, $log, securityRetryQueue, authResource, dialogService, $timeout, angularHelper) {

    var currentUser = null;
    var lastUserId = null;
    var loginDialog = null;
    //this tracks the last date/time that the user's remainingAuthSeconds was updated from the server
    // this is used so that we know when to go and get the user's remaining seconds directly.
    var lastServerTimeoutSet = null;

    // Redirect to the given url (defaults to '/')
    function redirect(url) {
        url = url || '/';
        $location.path(url);
    }

    function openLoginDialog(isTimedOut) {
        if (!loginDialog) {
            loginDialog = dialogService.open({
                template: 'views/common/dialogs/login.html',
                modalClass: "login-overlay",
                animation: "slide",
                show: true,
                callback: onLoginDialogClose,
                dialogData: {
                    isTimedOut: isTimedOut
                }
            });
        }
    }

    function onLoginDialogClose(success) {
        loginDialog = null;

        if (success) {
            securityRetryQueue.retryAll();
        } else {
            securityRetryQueue.cancelAll();
            redirect();
        }
    }

    /** 
    This methods will set the current user when it is resolved and 
    will then start the counter to count in-memory how many seconds they have 
    remaining on the auth session
    */
    function setCurrentUser(usr) {
        if (!usr.remainingAuthSeconds) {
            throw "The user object is invalid, the remainingAuthSeconds is required.";
        }
        currentUser = usr;
        lastServerTimeoutSet = new Date();
        //start the timer
        countdownUserTimeout();
    }
    
    /** 
    Method to count down the current user's timeout seconds, 
    this will continually count down their current remaining seconds every 2 seconds until
    there are no more seconds remaining.
    */
    function countdownUserTimeout() {
        $timeout(function() {
            if (currentUser) {
                //countdown by 2 seconds since that is how long our timer is for.
                currentUser.remainingAuthSeconds -= 2;

                //if there are more than 30 remaining seconds, recurse!
                if (currentUser.remainingAuthSeconds > 30) {

                    //we need to check when the last time the timeout was set from the server, if 
                    // it has been more than 30 seconds then we'll manually go and retreive it from the 
                    // server - this helps to keep our local countdown in check with the true timeout.
                    if (lastServerTimeoutSet != null) {
                        var now = new Date();
                        var seconds = (now.getTime() - lastServerTimeoutSet.getTime()) / 1000;
                        if (seconds > 30) {
                            //first we'll set the lastServerTimeoutSet to null - this is so we don't get back in to this loop while we 
                            // wait for a response from the server otherwise we'll be making double/triple/etc... calls while we wait.
                            lastServerTimeoutSet = null;
                            //now go get it from the server
                            authResource.getRemainingTimeoutSeconds().then(function(result) {
                                setUserTimeoutInternal(result);
                            });
                        }
                    }

                    //recurse the countdown!
                    countdownUserTimeout();
                }
                else {

                    //we are either timed out or very close to timing out so we need to show the login dialog.                    
                    //NOTE: the safeApply because our timeout is set to not run digests (performance reasons)
                    if (Umbraco.Sys.ServerVariables.umbracoSettings.keepUserLoggedIn !== true) {
                        angularHelper.safeApply($rootScope, function() {
                            userAuthExpired();
                        });
                    }
                    else {
                        //we've got less than 30 seconds remaining so let's check the server

                        if (lastServerTimeoutSet != null) {
                            //first we'll set the lastServerTimeoutSet to null - this is so we don't get back in to this loop while we 
                            // wait for a response from the server otherwise we'll be making double/triple/etc... calls while we wait.
                            lastServerTimeoutSet = null;
                            //now go get it from the server
                            authResource.getRemainingTimeoutSeconds().then(function (result) {
                                setUserTimeoutInternal(result);
                            });
                        }
                        
                        //recurse the countdown!
                        countdownUserTimeout();

                    }
                }
            }
        }, 2000, //every 2 seconds
            false); //false = do NOT execute a digest for every iteration
    }
    
    /** Called to update the current user's timeout */
    function setUserTimeoutInternal(newTimeout) {


        var asNumber = parseFloat(newTimeout);
        if (!isNaN(asNumber) && currentUser && angular.isNumber(asNumber)) {
            currentUser.remainingAuthSeconds = newTimeout;
            lastServerTimeoutSet = new Date();
        }
    }
    
    /** resets all user data, broadcasts the notAuthenticated event and shows the login dialog */
    function userAuthExpired(isLogout) {
        //store the last user id and clear the user
        if (currentUser && currentUser.id !== undefined) {
            lastUserId = currentUser.id;
        }

        if(currentUser){
            currentUser.remainingAuthSeconds = 0;
        }
        
        lastServerTimeoutSet = null;
        currentUser = null;
        
        //broadcast a global event that the user is no longer logged in
        $rootScope.$broadcast("notAuthenticated");

        openLoginDialog(isLogout === undefined ? true : !isLogout);
    }

    // Register a handler for when an item is added to the retry queue
    securityRetryQueue.onItemAddedCallbacks.push(function (retryItem) {
        if (securityRetryQueue.hasMore()) {
            userAuthExpired();
        }
    });

    return {

        /** Internal method to display the login dialog */
        _showLoginDialog: function () {
            openLoginDialog();
        },

        /** Returns a promise, sends a request to the server to check if the current cookie is authorized  */
        isAuthenticated: function () {
            //if we've got a current user then just return true
            if (currentUser) {
                var deferred = $q.defer();
                deferred.resolve(true);
                return deferred.promise;
            }
            return authResource.isAuthenticated();
        },

        /** Returns a promise, sends a request to the server to validate the credentials  */
        authenticate: function (login, password) {

            return authResource.performLogin(login, password)
                .then(function (data) {

                    //when it's successful, return the user data
                    setCurrentUser(data);

                    var result = { user: data, authenticated: true, lastUserId: lastUserId };

                    //broadcast a global event
                    $rootScope.$broadcast("authenticated", result);

                    return result;
                });
        },

        /** Logs the user out and redirects to the login page */
        logout: function () {
            return authResource.performLogout()
                .then(function (data) {

                    userAuthExpired();

                    $location.path("/login").search({check: false});

                    return null;
                });
        },

        /** Returns the current user object in a promise  */
        getCurrentUser: function (args) {
            var deferred = $q.defer();
            
            if (!currentUser) {
                authResource.getCurrentUser()
                    .then(function(data) {

                        var result = { user: data, authenticated: true, lastUserId: lastUserId };

                        if (args.broadcastEvent) {
                            //broadcast a global event, will inform listening controllers to load in the user specific data
                            $rootScope.$broadcast("authenticated", result);
                        }

                        setCurrentUser(data);
                        currentUser.avatar = 'http://www.gravatar.com/avatar/' + data.emailHash + '?s=40&d=404';
                        deferred.resolve(currentUser);
                    });

            }
            else {
                deferred.resolve(currentUser);
            }
            
            return deferred.promise;
        },
        
        /** Called whenever a server request is made that contains a x-umb-user-seconds response header for which we can update the user's remaining timeout seconds */
        setUserTimeout: function(newTimeout) {
            setUserTimeoutInternal(newTimeout);
        }
    };

});

/*Contains multiple services for various helper tasks */

/**
 * @ngdoc function
 * @name umbraco.services.umbModelMapper
 * @function
 *
 * @description
 * Utility class to map/convert models
 */
function umbModelMapper() {

    return {

        /** This converts the source model to a basic entity model, it will throw an exception if there isn't enough data to create the model */
        convertToEntityBasic: function (source) {
            var required = ["id", "name", "icon", "parentId", "path"];            
            _.each(required, function (k) {
                if (!_.has(source, k)) {
                    throw "The source object does not contain the property " + k;
                }
            });
            var optional = ["metaData", "key", "alias"];
            //now get the basic object
            var result = _.pick(source, required.concat(optional));
            return result;
        }

    };
}
angular.module('umbraco.services').factory('umbModelMapper', umbModelMapper);

/**
 * @ngdoc function
 * @name umbraco.services.umbSessionStorage
 * @function
 *
 * @description
 * Used to get/set things in browser sessionStorage but always prefixes keys with "umb_" and converts json vals so there is no overlap 
 * with any sessionStorage created by a developer.
 */
function umbSessionStorage($window) {

    //gets the sessionStorage object if available, otherwise just uses a normal object
    // - required for unit tests.
    var storage = $window['sessionStorage'] ? $window['sessionStorage'] : {};

    return {

        get: function (key) {
            console.log(storage);
            console.log(storage["umb_" + key]);
            return angular.fromJson(storage["umb_" + key]);
        },
        
        set : function(key, value) {
            storage["umb_" + key] = angular.toJson(value);
        }
        
    };
}
angular.module('umbraco.services').factory('umbSessionStorage', umbSessionStorage);

/**
 * @ngdoc function
 * @name umbraco.services.legacyJsLoader
 * @function
 *
 * @description
 * Used to lazy load in any JS dependencies that need to be manually loaded in
 */
function legacyJsLoader(assetsService, umbRequestHelper) {
    return {
        
        /** Called to load in the legacy tree js which is required on startup if a user is logged in or 
         after login, but cannot be called until they are authenticated which is why it needs to be lazy loaded. */
        loadLegacyTreeJs: function(scope) {
            return assetsService.loadJs(umbRequestHelper.getApiUrl("legacyTreeJs", "", ""), scope);
        }  
    };
}
angular.module('umbraco.services').factory('legacyJsLoader', legacyJsLoader);

/**
 * @ngdoc function
 * @name umbraco.services.updateChecker
 * @function
 *
 * @description
 * used to check for updates and display a notifcation
 */
function updateChecker($http, umbRequestHelper) {
    return {
        
        /** Called to load in the legacy tree js which is required on startup if a user is logged in or 
         after login, but cannot be called until they are authenticated which is why it needs to be lazy loaded. */
        check: function() {
                
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "updateCheckApiBaseUrl",
                       "GetCheck")),
               'Failed to retreive update status');
        }  
    };
}
angular.module('umbraco.services').factory('updateChecker', updateChecker);

/**
* @ngdoc service
* @name umbraco.services.umbPropertyEditorHelper
* @description A helper object used for property editors
**/
function umbPropEditorHelper() {
    return {
        /**
         * @ngdoc function
         * @name getImagePropertyValue
         * @methodOf umbraco.services.umbPropertyEditorHelper
         * @function    
         *
         * @description
         * Returns the correct view path for a property editor, it will detect if it is a full virtual path but if not then default to the internal umbraco one
         * 
         * @param {string} input the view path currently stored for the property editor
         */
        getViewPath: function(input, isPreValue) {
            var path = String(input);

            if (path.startsWith('/')) {

                //This is an absolute path, so just leave it
                return path;
            } else {

                if (path.indexOf("/") >= 0) {
                    //This is a relative path, so just leave it
                    return path;
                } else {
                    if (!isPreValue) {
                        //i.e. views/propertyeditors/fileupload/fileupload.html
                        return "views/propertyeditors/" + path + "/" + path + ".html";
                    } else {
                        //i.e. views/prevalueeditors/requiredfield.html
                        return "views/prevalueeditors/" + path + ".html";
                    }
                }

            }
        }
    };
}
angular.module('umbraco.services').factory('umbPropEditorHelper', umbPropEditorHelper);

/**
* @ngdoc service
* @name umbraco.services.imageHelper
* @description A helper object used for parsing image paths
**/
function imageHelper() {
    return {
        /** Returns the actual image path associated with the image property if there is one */
        getImagePropertyValue: function(options) {
            if (!options && !options.imageModel) {
                throw "The options objet does not contain the required parameters: imageModel";
            }

            
            //combine all props, TODO: we really need a better way then this
            var props = [];
            if(options.imageModel.properties){
                props = options.imageModel.properties;
            }else{
                $(options.imageModel.tabs).each(function(i, tab){
                    props = props.concat(tab.properties);
                });    
            }

            var mediaRoot = Umbraco.Sys.ServerVariables.umbracoSettings.mediaPath;
            var imageProp = _.find(props, function (item) {
                if(item.alias === "umbracoFile")
                {
                    return true;
                }

                //this performs a simple check to see if we have a media file as value
                //it doesnt catch everything, but better then nothing
                if(item.value.indexOf(mediaRoot) === 0){
                    return true;
                }

                return false;
            });
            
            if (!imageProp) {
                return "";
            }

            var imageVal;

            //our default images might store one or many images (as csv)
            var split = imageProp.value.split(',');
            var self = this;
            imageVal = _.map(split, function(item) {
                return { file: item, isImage: self.detectIfImageByExtension(item) };
            });
            
            //for now we'll just return the first image in the collection.
            //TODO: we should enable returning many to be displayed in the picker if the uploader supports many.
            if (imageVal.length && imageVal.length > 0 && imageVal[0].isImage) {
                return imageVal[0].file;
            }
            
            return "";
        },
        /** formats the display model used to display the content to the model used to save the content */
        getThumbnail: function (options) {
            
            if (!options && !options.imageModel) {
                throw "The options objet does not contain the required parameters: imageModel";
            }

            var imagePropVal = this.getImagePropertyValue(options);
            if (imagePropVal !== "") {
                return this.getThumbnailFromPath(imagePropVal);
            }
            return "";
        },

        scaleToMaxSize: function(maxSize, width, height){
            var retval = {width: width, height: height};

            var maxWidth = maxSize; // Max width for the image
            var maxHeight = maxSize;    // Max height for the image
            var ratio = 0;  // Used for aspect ratio
           
            // Check if the current width is larger than the max
            if(width > maxWidth){
                ratio = maxWidth / width;   // get ratio for scaling image
                
                retval.width = maxWidth;
                retval.height = height * ratio;

                height = height * ratio;    // Reset height to match scaled image
                width = width * ratio;    // Reset width to match scaled image
            }

            // Check if current height is larger than max
            if(height > maxHeight){
                ratio = maxHeight / height; // get ratio for scaling image

                retval.height = maxHeight;
                retval.width = width * ratio;
                width = width * ratio;    // Reset width to match scaled image
            }

            return retval;
        },

        getThumbnailFromPath: function(imagePath) {
            var ext = imagePath.substr(imagePath.lastIndexOf('.'));
            return imagePath.substr(0, imagePath.lastIndexOf('.')) + "_big-thumb" + ".jpg";
        },
        detectIfImageByExtension: function(imagePath) {
            var lowered = imagePath.toLowerCase();
            var ext = lowered.substr(lowered.lastIndexOf(".") + 1);
            return ("," + Umbraco.Sys.ServerVariables.umbracoSettings.imageFileTypes + ",").indexOf("," + ext + ",") !== -1;
        }
    };
}
angular.module('umbraco.services').factory('imageHelper', imageHelper);

/**
* @ngdoc service
* @name umbraco.services.umbDataFormatter
* @description A helper object used to format/transform JSON Umbraco data, mostly used for persisting data to the server
**/
function umbDataFormatter() {
    return {
        
        /** formats the display model used to display the data type to the model used to save the data type */
        formatDataTypePostData: function(displayModel, preValues, action) {
            var saveModel = {
                parentId: -1,
                id: displayModel.id,
                name: displayModel.name,
                selectedEditor: displayModel.selectedEditor,
                //set the action on the save model
                action: action,
                preValues: []
            };
            for (var i = 0; i < preValues.length; i++) {

                saveModel.preValues.push({
                    key: preValues[i].alias,
                    value: preValues[i].value
                });
            }
            return saveModel;
        },

        /** formats the display model used to display the member to the model used to save the member */
        formatMemberPostData: function(displayModel, action) {
            //this is basically the same as for media but we need to explicitly add the username,email, password to the save model

            var saveModel = this.formatMediaPostData(displayModel, action);

            saveModel.key = displayModel.key;
            
            var genericTab = _.find(displayModel.tabs, function (item) {
                return item.id === 0;
            });

            //map the member login, email, password and groups
            var propLogin = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_login";
            });
            var propEmail = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_email";
            });
            var propPass = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_password";
            });
            var propGroups = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_membergroup";
            });
            saveModel.email = propEmail.value;
            saveModel.username = propLogin.value;
            saveModel.password = propPass.value;
            
            var selectedGroups = [];
            for (var n in propGroups.value) {
                if (propGroups.value[n] === true) {
                    selectedGroups.push(n);
                }
            }
            saveModel.memberGroups = selectedGroups;
            
            //turn the dictionary into an array of pairs
            var memberProviderPropAliases = _.pairs(displayModel.fieldConfig);
            _.each(displayModel.tabs, function (tab) {
                _.each(tab.properties, function (prop) {
                    var foundAlias = _.find(memberProviderPropAliases, function(item) {
                        return prop.alias === item[1];
                    });
                    if (foundAlias) {
                        //we know the current property matches an alias, now we need to determine which membership provider property it was for
                        // by looking at the key
                        switch (foundAlias[0]) {
                            case "umbracoLockPropertyTypeAlias":
                                saveModel.isLockedOut = prop.value.toString() === "1" ? true : false;
                                break;
                            case "umbracoApprovePropertyTypeAlias":
                                saveModel.isApproved = prop.value.toString() === "1" ? true : false;
                                break;
                            case "umbracoCommentPropertyTypeAlias":
                                saveModel.comments = prop.value;
                                break;
                        }
                    }                
                });
            });



            return saveModel;
        },

        /** formats the display model used to display the media to the model used to save the media */
        formatMediaPostData: function(displayModel, action) {
            //NOTE: the display model inherits from the save model so we can in theory just post up the display model but 
            // we don't want to post all of the data as it is unecessary.
            var saveModel = {
                id: displayModel.id,
                properties: [],
                name: displayModel.name,
                contentTypeAlias: displayModel.contentTypeAlias,
                parentId: displayModel.parentId,
                //set the action on the save model
                action: action
            };

            _.each(displayModel.tabs, function (tab) {

                _.each(tab.properties, function (prop) {

                    //don't include the custom generic tab properties
                    if (!prop.alias.startsWith("_umb_")) {
                        saveModel.properties.push({
                            id: prop.id,
                            alias: prop.alias,
                            value: prop.value
                        });
                    }
                    
                });
            });

            return saveModel;
        },

        /** formats the display model used to display the content to the model used to save the content  */
        formatContentPostData: function (displayModel, action) {

            //this is basically the same as for media but we need to explicitly add some extra properties
            var saveModel = this.formatMediaPostData(displayModel, action);

            var genericTab = _.find(displayModel.tabs, function (item) {
                return item.id === 0;
            });
            
            var propExpireDate = _.find(genericTab.properties, function(item) {
                return item.alias === "_umb_expiredate";
            });
            var propReleaseDate = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_releasedate";
            });
            var propTemplate = _.find(genericTab.properties, function (item) {
                return item.alias === "_umb_template";
            });
            saveModel.expireDate = propExpireDate.value;
            saveModel.releaseDate = propReleaseDate.value;
            saveModel.templateAlias = propTemplate.value;

            return saveModel;
        }
    };
}
angular.module('umbraco.services').factory('umbDataFormatter', umbDataFormatter);

/**
* @ngdoc service
* @name umbraco.services.iconHelper
* @description A helper service for dealing with icons, mostly dealing with legacy tree icons
**/
function iconHelper($q, $timeout) {

    var converter = [
        { oldIcon: ".sprNew", newIcon: "add" },
        { oldIcon: ".sprDelete", newIcon: "remove" },
        { oldIcon: ".sprMove", newIcon: "enter" },
        { oldIcon: ".sprCopy", newIcon: "documents" },
        { oldIcon: ".sprSort", newIcon: "navigation-vertical" },
        { oldIcon: ".sprPublish", newIcon: "globe" },
        { oldIcon: ".sprRollback", newIcon: "undo" },
        { oldIcon: ".sprProtect", newIcon: "lock" },
        { oldIcon: ".sprAudit", newIcon: "time" },
        { oldIcon: ".sprNotify", newIcon: "envelope" },
        { oldIcon: ".sprDomain", newIcon: "home" },
        { oldIcon: ".sprPermission", newIcon: "lock" },
        { oldIcon: ".sprRefresh", newIcon: "refresh" },
        { oldIcon: ".sprBinEmpty", newIcon: "trash" },
        { oldIcon: ".sprExportDocumentType", newIcon: "download-alt" },
        { oldIcon: ".sprImportDocumentType", newIcon: "page-up" },
        { oldIcon: ".sprLiveEdit", newIcon: "edit" },
        { oldIcon: ".sprCreateFolder", newIcon: "add" },
        { oldIcon: ".sprPackage2", newIcon: "box" },
        { oldIcon: ".sprLogout", newIcon: "logout" },
        { oldIcon: ".sprSave", newIcon: "save" },
        { oldIcon: ".sprSendToTranslate", newIcon: "envelope-alt" },
        { oldIcon: ".sprToPublish", newIcon: "mail-forward" },
        { oldIcon: ".sprTranslate", newIcon: "comments" },
        { oldIcon: ".sprUpdate", newIcon: "save" },
        
        { oldIcon: ".sprTreeSettingDomain", newIcon: "icon-home" },
        { oldIcon: ".sprTreeDoc", newIcon: "icon-document" },
        { oldIcon: ".sprTreeDoc2", newIcon: "icon-diploma-alt" },
        { oldIcon: ".sprTreeDoc3", newIcon: "icon-notepad" },
        { oldIcon: ".sprTreeDoc4", newIcon: "icon-newspaper-alt" },
        { oldIcon: ".sprTreeDoc5", newIcon: "icon-notepad-alt" },

        { oldIcon: ".sprTreeDocPic", newIcon: "icon-picture" },        
        { oldIcon: ".sprTreeFolder", newIcon: "icon-folder" },
        { oldIcon: ".sprTreeFolder_o", newIcon: "icon-folder" },
        { oldIcon: ".sprTreeMediaFile", newIcon: "icon-music" },
        { oldIcon: ".sprTreeMediaMovie", newIcon: "icon-movie" },
        { oldIcon: ".sprTreeMediaPhoto", newIcon: "icon-picture" },
        
        { oldIcon: ".sprTreeMember", newIcon: "icon-user" },
        { oldIcon: ".sprTreeMemberGroup", newIcon: "icon-users" },
        { oldIcon: ".sprTreeMemberType", newIcon: "icon-users" },
        
        { oldIcon: ".sprTreeNewsletter", newIcon: "icon-file-text-alt" },
        { oldIcon: ".sprTreePackage", newIcon: "icon-box" },
        { oldIcon: ".sprTreeRepository", newIcon: "icon-server-alt" },
        
        { oldIcon: ".sprTreeSettingDataType", newIcon: "icon-autofill" },

        //TODO:
        /*
        { oldIcon: ".sprTreeSettingAgent", newIcon: "" },
        { oldIcon: ".sprTreeSettingCss", newIcon: "" },
        { oldIcon: ".sprTreeSettingCssItem", newIcon: "" },
        
        { oldIcon: ".sprTreeSettingDataTypeChild", newIcon: "" },
        { oldIcon: ".sprTreeSettingDomain", newIcon: "" },
        { oldIcon: ".sprTreeSettingLanguage", newIcon: "" },
        { oldIcon: ".sprTreeSettingScript", newIcon: "" },
        { oldIcon: ".sprTreeSettingTemplate", newIcon: "" },
        { oldIcon: ".sprTreeSettingXml", newIcon: "" },
        { oldIcon: ".sprTreeStatistik", newIcon: "" },
        { oldIcon: ".sprTreeUser", newIcon: "" },
        { oldIcon: ".sprTreeUserGroup", newIcon: "" },
        { oldIcon: ".sprTreeUserType", newIcon: "" },
        */

        { oldIcon: "folder.png", newIcon: "icon-folder" },
        { oldIcon: "mediaphoto.gif", newIcon: "icon-picture" },
        { oldIcon: "mediafile.gif", newIcon: "icon-document" },

        { oldIcon: ".sprTreeDeveloperCacheItem", newIcon: "icon-box" },
        { oldIcon: ".sprTreeDeveloperCacheTypes", newIcon: "icon-box" },
        { oldIcon: ".sprTreeDeveloperMacro", newIcon: "icon-cogs" },
        { oldIcon: ".sprTreeDeveloperRegistry", newIcon: "icon-windows" },
        { oldIcon: ".sprTreeDeveloperPython", newIcon: "icon-linux" }
    ];

    var imageConverter = [
            {oldImage: "contour.png", newIcon: "icon-umb-contour"}
            ];

    var collectedIcons;
            
    return {
        
        /** Used by the create dialogs for content/media types to format the data so that the thumbnails are styled properly */
        formatContentTypeThumbnails: function (contentTypes) {
            for (var i = 0; i < contentTypes.length; i++) {

                if (contentTypes[i].thumbnailIsClass === undefined || contentTypes[i].thumbnailIsClass) {
                    contentTypes[i].cssClass = this.convertFromLegacyIcon(contentTypes[i].thumbnail);
                }else {
                    contentTypes[i].style = "background-image: url('" + contentTypes[i].thumbnailFilePath + "');height:36px; background-position:4px 0px; background-repeat: no-repeat;background-size: 35px 35px;";
                    //we need an 'icon-' class in there for certain styles to work so if it is image based we'll add this
                    contentTypes[i].cssClass = "custom-file";
                }
            }
            return contentTypes;
        },
        formatContentTypeIcons: function (contentTypes) {
            for (var i = 0; i < contentTypes.length; i++) {
                contentTypes[i].icon = this.convertFromLegacyIcon(contentTypes[i].icon);

                //couldnt find replacement
                if(contentTypes[i].icon.indexOf(".") > 0){
                     contentTypes[i].icon = "icon-document-dashed-line";   
                }
            }
            return contentTypes;
        },
        /** If the icon is file based (i.e. it has a file path) */
        isFileBasedIcon: function (icon) {
            //if it doesn't start with a '.' but contains one then we'll assume it's file based
            if (icon.startsWith('..') || (!icon.startsWith('.') && icon.indexOf('.') > 1)) {
                return true;
            }
            return false;
        },
        /** If the icon is legacy */
        isLegacyIcon: function (icon) {
            if(icon.startsWith('..')){
                return false;
            }

            if (icon.startsWith('.')) {
                return true;
            }
            return false;
        },
        /** If the tree node has a legacy icon */
        isLegacyTreeNodeIcon: function(treeNode){
            if (treeNode.iconIsClass) {
                return this.isLegacyIcon(treeNode.icon);
            }
            return false;
        },

        /** Return a list of icons, optionally filter them */
        /** It fetches them directly from the active stylesheets in the browser */
        getIcons: function(){
            var deferred = $q.defer();
            $timeout(function(){
                if(collectedIcons){
                    deferred.resolve(collectedIcons);
                }else{
                    collectedIcons = [];
                    var c = ".icon-";

                    for (var i = document.styleSheets.length - 1; i >= 0; i--) {
                        var classes = document.styleSheets[i].rules || document.styleSheets[i].cssRules;
                        
                        for(var x=0;x<classes.length;x++) {
                            var cur = classes[x];
                            if(cur.selectorText && cur.selectorText.indexOf(c) === 0) {
                                var s = cur.selectorText.substring(1);
                                var hasSpace = s.indexOf(" ");
                                if(hasSpace>0){
                                    s = s.substring(0, hasSpace);
                                }
                                var hasPseudo = s.indexOf(":");
                                if(hasPseudo>0){
                                    s = s.substring(0, hasPseudo);
                                }

                                if(collectedIcons.indexOf(s) < 0){
                                    collectedIcons.push(s);
                                }
                            }
                        }
                    }
                    deferred.resolve(collectedIcons);
                }
            }, 100);
            
            return deferred.promise;
        },

        /** Converts the icon from legacy to a new one if an old one is detected */
        convertFromLegacyIcon: function (icon) {
            if (this.isLegacyIcon(icon)) {
                //its legacy so convert it if we can
                var found = _.find(converter, function (item) {
                    return item.oldIcon.toLowerCase() === icon.toLowerCase();
                });
                return (found ? found.newIcon : icon);
            }
            return icon;
        },

        convertFromLegacyImage: function (icon) {
                var found = _.find(imageConverter, function (item) {
                    return item.oldImage.toLowerCase() === icon.toLowerCase();
                });
                return (found ? found.newIcon : undefined);
        },

        /** If we detect that the tree node has legacy icons that can be converted, this will convert them */
        convertFromLegacyTreeNodeIcon: function (treeNode) {
            if (this.isLegacyTreeNodeIcon(treeNode)) {
                return this.convertFromLegacyIcon(treeNode.icon);
            }
            return treeNode.icon;
        }
    };
}
angular.module('umbraco.services').factory('iconHelper', iconHelper);




/**
 * @ngdoc service
 * @name umbraco.services.xmlhelper
 * @function
 *
 * @description
 * Used to convert legacy xml data to json and back again
 */
function xmlhelper($http) {
    /*
     Copyright 2011 Abdulla Abdurakhmanov
     Original sources are available at https://code.google.com/p/x2js/

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
     */

    function X2JS() {
            var VERSION = "1.0.11";
            var escapeMode = false;

            var DOMNodeTypes = {
                    ELEMENT_NODE       : 1,
                    TEXT_NODE          : 3,
                    CDATA_SECTION_NODE : 4,
                    DOCUMENT_NODE      : 9
            };
            
            function getNodeLocalName( node ) {
                    var nodeLocalName = node.localName;                     
                    if(nodeLocalName == null){
                        nodeLocalName = node.baseName;
                    } // Yeah, this is IE!! 
                            
                    if(nodeLocalName === null || nodeLocalName===""){
                        nodeLocalName = node.nodeName;
                    } // =="" is IE too
                            
                    return nodeLocalName;
            }
            
            function getNodePrefix(node) {
                    return node.prefix;
            }
                    
            function escapeXmlChars(str) {
                    if(typeof(str) === "string"){
                            return str.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#x27;').replace(/\//g, '&#x2F;');
                    }else{
                        return str;
                    }
            }

            function unescapeXmlChars(str) {
                    return str.replace(/&amp;/g, '&').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"').replace(/&#x27;/g, "'").replace(/&#x2F;/g, '\/');
            }      

            function parseDOMChildren( node ) {
                    var result,child, childName;

                    if(node.nodeType === DOMNodeTypes.DOCUMENT_NODE) {
                            result = {};
                            child = node.firstChild;
                            childName = getNodeLocalName(child);
                            result[childName] = parseDOMChildren(child);
                            return result;
                    }
                    else{

                    if(node.nodeType === DOMNodeTypes.ELEMENT_NODE) {
                            result = {};
                            result.__cnt=0;
                            var nodeChildren = node.childNodes;
                            
                            // Children nodes
                            for(var cidx=0; cidx <nodeChildren.length; cidx++) {
                                    child = nodeChildren.item(cidx); // nodeChildren[cidx];
                                    childName = getNodeLocalName(child);
                                    
                                    result.__cnt++;
                                    if(result[childName] === null) {
                                            result[childName] = parseDOMChildren(child);
                                            result[childName+"_asArray"] = new Array(1);
                                            result[childName+"_asArray"][0] = result[childName];
                                    }
                                    else {
                                            if(result[childName] !== null) {
                                                    if( !(result[childName] instanceof Array)) {
                                                            var tmpObj = result[childName];
                                                            result[childName] = [];
                                                            result[childName][0] = tmpObj;
                                                            
                                                            result[childName+"_asArray"] = result[childName];
                                                    }
                                            }
                                            var aridx = 0;
                                            while(result[childName][aridx]!==null){
                                                aridx++;
                                            } 

                                            (result[childName])[aridx] = parseDOMChildren(child);
                                    }                       
                            }
                            
                            // Attributes
                            for(var aidx=0; aidx <node.attributes.length; aidx++) {
                                    var attr = node.attributes.item(aidx); // [aidx];
                                    result.__cnt++;
                                    result["_"+attr.name]=attr.value;
                            }
                            
                            // Node namespace prefix
                            var nodePrefix = getNodePrefix(node);
                            if(nodePrefix!==null && nodePrefix!=="") {
                                    result.__cnt++;
                                    result.__prefix=nodePrefix;
                            }
                            
                            if( result.__cnt === 1 && result["#text"]!==null  ) {
                                    result = result["#text"];
                            }
                            
                            if(result["#text"]!==null) {
                                    result.__text = result["#text"];
                                    if(escapeMode){
                                        result.__text = unescapeXmlChars(result.__text);
                                    }
                                            
                                    delete result["#text"];
                                    delete result["#text_asArray"];
                            }
                            if(result["#cdata-section"]!=null) {
                                    result.__cdata = result["#cdata-section"];
                                    delete result["#cdata-section"];
                                    delete result["#cdata-section_asArray"];
                            }
                            
                            if(result.__text!=null || result.__cdata!=null) {
                                    result.toString = function() {
                                            return (this.__text!=null? this.__text:'')+( this.__cdata!=null ? this.__cdata:'');
                                    };
                            }
                            return result;
                    }
                    else{
                        if(node.nodeType === DOMNodeTypes.TEXT_NODE || node.nodeType === DOMNodeTypes.CDATA_SECTION_NODE) {
                                return node.nodeValue;
                        } 
                    }
                }     
            }
            
            function startTag(jsonObj, element, attrList, closed) {
                    var resultStr = "<"+ ( (jsonObj!=null && jsonObj.__prefix!=null)? (jsonObj.__prefix+":"):"") + element;
                    if(attrList!=null) {
                            for(var aidx = 0; aidx < attrList.length; aidx++) {
                                    var attrName = attrList[aidx];
                                    var attrVal = jsonObj[attrName];
                                    resultStr+=" "+attrName.substr(1)+"='"+attrVal+"'";
                            }
                    }
                    if(!closed){
                        resultStr+=">";
                    }else{
                        resultStr+="/>";
                    }
                            
                    return resultStr;
            }
            
            function endTag(jsonObj,elementName) {
                    return "</"+ (jsonObj.__prefix!==null? (jsonObj.__prefix+":"):"")+elementName+">";
            }
            
            function endsWith(str, suffix) {
                return str.indexOf(suffix, str.length - suffix.length) !== -1;
            }
            
            function jsonXmlSpecialElem ( jsonObj, jsonObjField ) {
                    if(endsWith(jsonObjField.toString(),("_asArray")) || jsonObjField.toString().indexOf("_")===0 || (jsonObj[jsonObjField] instanceof Function) ){
                        return true;
                    }else{
                        return false;
                    }
            }
            
            function jsonXmlElemCount ( jsonObj ) {
                    var elementsCnt = 0;
                    if(jsonObj instanceof Object ) {
                            for( var it in jsonObj  ) {
                                    if(jsonXmlSpecialElem ( jsonObj, it) ){
                                        continue;
                                    }                            
                                    elementsCnt++;
                            }
                    }
                    return elementsCnt;
            }
            
            function parseJSONAttributes ( jsonObj ) {
                    var attrList = [];
                    if(jsonObj instanceof Object ) {
                            for( var ait in jsonObj  ) {
                                    if(ait.toString().indexOf("__")=== -1 && ait.toString().indexOf("_")===0) {
                                            attrList.push(ait);
                                    }
                            }
                    }

                    return attrList;
            }
            
            function parseJSONTextAttrs ( jsonTxtObj ) {
                    var result ="";
                    
                    if(jsonTxtObj.__cdata!=null) {                                                                          
                            result+="<![CDATA["+jsonTxtObj.__cdata+"]]>";                                   
                    }
                    
                    if(jsonTxtObj.__text!=null) {                   
                            if(escapeMode){
                               result+=escapeXmlChars(jsonTxtObj.__text);     
                            }else{
                                result+=jsonTxtObj.__text;
                            } 
                    }
                    return result;
            }
            
            function parseJSONTextObject ( jsonTxtObj ) {
                    var result ="";

                    if( jsonTxtObj instanceof Object ) {
                            result+=parseJSONTextAttrs ( jsonTxtObj );
                    }
                    else{
                        if(jsonTxtObj!=null) {
                                if(escapeMode){
                                   result+=escapeXmlChars(jsonTxtObj);     
                                }else{
                                    result+=jsonTxtObj;
                                }
                        }
                    }
                            
                    
                    return result;
            }
            
            function parseJSONArray ( jsonArrRoot, jsonArrObj, attrList ) {
                    var result = ""; 
                    if(jsonArrRoot.length === 0) {
                            result+=startTag(jsonArrRoot, jsonArrObj, attrList, true);
                    }
                    else {
                            for(var arIdx = 0; arIdx < jsonArrRoot.length; arIdx++) {
                                    result+=startTag(jsonArrRoot[arIdx], jsonArrObj, parseJSONAttributes(jsonArrRoot[arIdx]), false);
                                    result+=parseJSONObject(jsonArrRoot[arIdx]);
                                    result+=endTag(jsonArrRoot[arIdx],jsonArrObj);                                          
                            }
                    }
                    return result;
            }
            
            function parseJSONObject ( jsonObj ) {
                    var result = "";        

                    var elementsCnt = jsonXmlElemCount ( jsonObj );
                    
                    if(elementsCnt > 0) {
                            for( var it in jsonObj ) {
                                if(jsonXmlSpecialElem ( jsonObj, it) ){
                                    continue;
                                }                            
                                
                                var subObj = jsonObj[it];                                               
                                var attrList = parseJSONAttributes( subObj );
                                
                                if(subObj === null || subObj === undefined) {
                                        result+=startTag(subObj, it, attrList, true);
                                }else{
                                    if(subObj instanceof Object) {
                                            
                                            if(subObj instanceof Array) {                                   
                                                    result+=parseJSONArray( subObj, it, attrList );
                                            }else {
                                                    var subObjElementsCnt = jsonXmlElemCount ( subObj );
                                                    if(subObjElementsCnt > 0 || subObj.__text!==null || subObj.__cdata!==null) {
                                                            result+=startTag(subObj, it, attrList, false);
                                                            result+=parseJSONObject(subObj);
                                                            result+=endTag(subObj,it);
                                                    }else{
                                                            result+=startTag(subObj, it, attrList, true);
                                                    }
                                            }

                                    }else {
                                            result+=startTag(subObj, it, attrList, false);
                                            result+=parseJSONTextObject(subObj);
                                            result+=endTag(subObj,it);
                                    }
                                }
                            }
                    }
                    result+=parseJSONTextObject(jsonObj);
                    
                    return result;
            }
            
            this.parseXmlString = function(xmlDocStr) {
                    var xmlDoc;
                    if (window.DOMParser) {
                            var parser=new window.DOMParser();
                            xmlDoc = parser.parseFromString( xmlDocStr, "text/xml" );
                    }
                    else {
                            // IE :(
                            if(xmlDocStr.indexOf("<?")===0) {
                                    xmlDocStr = xmlDocStr.substr( xmlDocStr.indexOf("?>") + 2 );
                            }
                            xmlDoc=new ActiveXObject("Microsoft.XMLDOM");
                            xmlDoc.async="false";
                            xmlDoc.loadXML(xmlDocStr);
                    }
                    return xmlDoc;
            };

            this.xml2json = function (xmlDoc) {
                    return parseDOMChildren ( xmlDoc );
            };
            
            this.xml_str2json = function (xmlDocStr) {
                    var xmlDoc = this.parseXmlString(xmlDocStr);    
                    return this.xml2json(xmlDoc);
            };

            this.json2xml_str = function (jsonObj) {
                    return parseJSONObject ( jsonObj );
            };

            this.json2xml = function (jsonObj) {
                    var xmlDocStr = this.json2xml_str (jsonObj);
                    return this.parseXmlString(xmlDocStr);
            };

            this.getVersion = function () {
                    return VERSION;
            };

            this.escapeMode = function(enabled) {
                    escapeMode = enabled;
            };
    }

    var x2js = new X2JS();
    return {
        /** Called to load in the legacy tree js which is required on startup if a user is logged in or 
         after login, but cannot be called until they are authenticated which is why it needs to be lazy loaded. */
        toJson: function(xml) {
            var json = x2js.xml_str2json( xml );
            return json;
        },
        fromJson: function(json) {
            var xml = x2js.json2xml_str( json );
            return xml;
        },
        parseFeed: function (url) {             
            return $http.jsonp('//ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=50&callback=JSON_CALLBACK&q=' + encodeURIComponent(url));         
        }
    };
}
angular.module('umbraco.services').factory('xmlhelper', xmlhelper);


})();