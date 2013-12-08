/*! umbraco - v7.0.0-Beta - 2013-11-21
 * https://github.com/umbraco/umbraco-cms/tree/7.0.0
 * Copyright (c) 2013 Umbraco HQ;
 * Licensed MIT
 */

(function() { 

angular.module("umbraco.directives", ["umbraco.directives.editors", "umbraco.directives.html", "umbraco.directives.validation", "ui.sortable"]);
angular.module("umbraco.directives.editors", []);
angular.module("umbraco.directives.html", []);
angular.module("umbraco.directives.validation", []);
/**
 * @ngdoc directive
 * @name umbraco.directives.directive:autoScale
 * @element div
 * @function
 *
 * @description
 * Resize div's automatically to fit to the bottom of the screen, as an optional parameter an y-axis offset can be set
 * So if you only want to scale the div to 70 pixels from the bottom you pass "70"
 *
 * @example
   <example module="umbraco.directives">
     <file name="index.html">
         <div auto-scale="70" class="input-block-level"></div>
     </file>
   </example>
 */
angular.module("umbraco.directives")
  .directive('autoScale', function ($window) {
    return function (scope, el, attrs) {

      var totalOffset = 0;
      var offsety = parseInt(attrs.autoScale, 10);
      var window = angular.element($window);
      if (offsety !== undefined){
        totalOffset += offsety;
      }

      setTimeout(function () {
        el.height(window.height() - (el.offset().top + totalOffset));
      }, 500);

      window.bind("resize", function () {
        el.height(window.height() - (el.offset().top + totalOffset));
      });

    };
  });
angular.module('umbraco.directives.editors').directive('ace', function(assetsService) {
  var ACE_EDITOR_CLASS = 'ace-editor';

  function loadAceEditor(element, mode) {
    assetsService.loadJs("lib/ace/noconflict/ace.js").then(function(){
        var editor = ace.edit($(element).find('.' + ACE_EDITOR_CLASS)[0]);
        editor.session.setMode("ace/mode/" + mode);
        editor.renderer.setShowPrintMargin(false);
        return editor;
    });
  }

  function valid(editor) {
    return (Object.keys(editor.getSession().getAnnotations()).length === 0);
  }

  return {
    restrict: 'A',
    require: '?ngModel',
    transclude: true,
    template: '<div class="transcluded" ng-transclude></div><div class="' + ACE_EDITOR_CLASS + '"></div>',

    link: function(scope, element, attrs, ngModel) {
      function read() {
        ngModel.$setViewValue(editor.getValue());
        textarea.val(editor.getValue());
      }

      var textarea = $(element).find('textarea');
      textarea.hide();

      var mode = attrs.ace;
      var editor = loadAceEditor(element, mode);
      scope.ace = editor;

      if (!ngModel)
      {
        return; // do nothing if no ngModel
      }

      ngModel.$render = function() {
        var value = ngModel.$viewValue || '';
        editor.getSession().setValue(value);
        textarea.val(value);
      };

      editor.getSession().on('changeAnnotation', function() {
        if (valid(editor)) {
          scope.$apply(read);
        }
      });

      editor.getSession().setValue(textarea.val());

      read();

    }
  };
});

/**
* @ngdoc directive
* @name umbraco.directives.directive:umbContentName 
* @restrict E
* @function
* @description 
* Used by editors that require naming an entity. Shows a textbox/headline with a required validator within it's own form.
**/
angular.module("umbraco.directives")
	.directive('umbContentName', function ($timeout, localizationService) {
	    return {
	        require: "ngModel",
			restrict: 'E',
			replace: true,
			templateUrl: 'views/directives/umb-content-name.html',
			scope: {
			    placeholder: '@placeholder',
			    model: '=ngModel'
			},
			link: function(scope, element, attrs, ngModel) {

				var inputElement = element.find("input");
				if(scope.placeholder && scope.placeholder[0] === "@"){
					localizationService.localize(scope.placeholder.substring(1))
						.then(function(value){
							scope.placeholder = value;	
						});
				}

				$timeout(function(){
					if(!scope.model){
						scope.goEdit();
					}
				}, 100, false);
			
				scope.goEdit = function(){
					scope.editMode = true;

					$timeout(function () {					    
					    inputElement.focus();					    
					}, 100, false);
				};

				scope.exitEdit = function(){
					if(scope.model && scope.model !== ""){
						scope.editMode = false;	
					}
				};
			}
	    };
	});
/**
 * @ngdoc directive
 * @name umbraco.directives.directive:umbSort
 * @element div
 * @function
 *
 * @description
 * Resize div's automatically to fit to the bottom of the screen, as an optional parameter an y-axis offset can be set
 * So if you only want to scale the div to 70 pixels from the bottom you pass "70"
 *
 * @example
   <example module="umbraco.directives">
     <file name="index.html">
         <div umb-sort="70" class="input-block-level"></div>
     </file>
   </example>
 */
angular.module("umbraco.directives")
  .value('umbSortContextInternal',{})
  .directive('umbSort', function($log,umbSortContextInternal) {
          return {
            require: '?ngModel',
            link: function(scope, element, attrs, ngModel) {
                var adjustment;
            
                var cfg = scope.$eval(element.attr('umb-sort')) || {};

                scope.model = ngModel;

                scope.opts = cfg;
                scope.opts.containerSelector= cfg.containerSelector || ".umb-" + cfg.group + "-container",
                scope.opts.nested= cfg.nested || true,
                scope.opts.drop= cfg.drop || true,
                scope.opts.drag= cfg.drag || true,
                scope.opts.clone = cfg.clone || "<li/>";
                scope.opts.mode = cfg.mode || "list";

                scope.opts.itemSelectorFull = $.trim(scope.opts.itemPath + " " + scope.opts.itemSelector);

                /*
                scope.opts.isValidTarget = function(item, container) {
                        if(container.el.is(".umb-" + scope.opts.group + "-container")){
                            return true;
                        }
                        return false;
                     };
                */

                element.addClass("umb-sort");
                element.addClass("umb-" + cfg.group + "-container");

                scope.opts.onDrag = function (item, position)  {
                    if(scope.opts.mode === "list"){
                      item.css({
                            left: position.left - adjustment.left,
                            top: position.top - adjustment.top
                          });  
                    }
                };


                scope.opts.onDrop = function (item, targetContainer, _super)  {
                      
                      if(scope.opts.mode === "list"){
                        //list mode
                        var clonedItem = $(scope.opts.clone).css({height: 0});
                        item.after(clonedItem);
                        clonedItem.animate({'height': item.height()});
                        
                        item.animate(clonedItem.position(), function  () {
                           clonedItem.detach();
                           _super(item);
                        });
                      }

                      var children = $(scope.opts.itemSelectorFull, targetContainer.el);
                      var targetIndex = children.index(item);
                      var targetScope = $(targetContainer.el[0]).scope();
                      

                      if(targetScope === umbSortContextInternal.sourceScope){
                          if(umbSortContextInternal.sourceScope.opts.onSortHandler){
                              var _largs = {
                                oldIndex: umbSortContextInternal.sourceIndex,
                                newIndex: targetIndex,
                                scope: umbSortContextInternal.sourceScope
                              };

                              umbSortContextInternal.sourceScope.opts.onSortHandler.call(this, item, _largs);
                          }
                      }else{
                        

                        if(targetScope.opts.onDropHandler){
                            var args = {
                              sourceScope: umbSortContextInternal.sourceScope,
                              sourceIndex: umbSortContextInternal.sourceIndex,
                              sourceContainer: umbSortContextInternal.sourceContainer,

                              targetScope: targetScope,
                              targetIndex: targetIndex,
                              targetContainer: targetContainer
                            };   

                            targetScope.opts.onDropHandler.call(this, item, args);
                        }

                        if(umbSortContextInternal.sourceScope.opts.onReleaseHandler){
                            var _args = {
                              sourceScope: umbSortContextInternal.sourceScope,
                              sourceIndex: umbSortContextInternal.sourceIndex,
                              sourceContainer: umbSortContextInternal.sourceContainer,

                              targetScope: targetScope,
                              targetIndex: targetIndex,
                              targetContainer: targetContainer
                            };

                            umbSortContextInternal.sourceScope.opts.onReleaseHandler.call(this, item, _args);
                        }
                      }
                };

                scope.changeIndex = function(from, to){
                    scope.$apply(function(){
                      var i = ngModel.$modelValue.splice(from, 1)[0];
                      ngModel.$modelValue.splice(to, 0, i);
                    });
                };

                scope.move = function(args){
                    var from = args.sourceIndex;
                    var to = args.targetIndex;

                    if(args.sourceContainer === args.targetContainer){
                        scope.changeIndex(from, to);
                    }else{
                      scope.$apply(function(){
                        var i = args.sourceScope.model.$modelValue.splice(from, 1)[0];
                        args.targetScope.model.$modelvalue.splice(to,0, i);
                      });
                    }
                };

                scope.opts.onDragStart = function (item, container, _super) {
                      var children = $(scope.opts.itemSelectorFull, container.el);
                      var offset = item.offset();
                      
                      umbSortContextInternal.sourceIndex = children.index(item);
                      umbSortContextInternal.sourceScope = $(container.el[0]).scope();
                      umbSortContextInternal.sourceContainer = container;

                      //current.item = ngModel.$modelValue.splice(current.index, 1)[0];

                      var pointer = container.rootGroup.pointer;
                      adjustment = {
                        left: pointer.left - offset.left,
                        top: pointer.top - offset.top
                      };

                      _super(item, container);
                };
                  
                element.sortable( scope.opts );
             }
          };

        });

/**
* @ngdoc directive
* @name umbraco.directives.directive:fixNumber
* @restrict A
* @description Used in conjunction with type='number' input fields to ensure that the bound value is converted to a number when using ng-model
*  because normally it thinks it's a string and also validation doesn't work correctly due to an angular bug.
**/
function fixNumber() {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attr, ngModel) {

            //This fixes the issue of when your model contains a number as a string (i.e. "1" instead of 1)
            // which will not actually work on initial load and the browser will say you have an invalid number 
            // entered. So if it parses to a number, we call setViewValue which sets the bound model value
            // to the real number. It should in theory update the view but it doesn't so we need to manually set
            // the element's value. I'm sure there's a bug logged for this somewhere for angular too.

            var modelVal = scope.$eval(attr.ngModel);
            if (modelVal) {
                var asNum = parseFloat(modelVal, 10);
                if (!isNaN(asNum)) {                    
                    ngModel.$setViewValue(asNum);
                    element.val(asNum);
                }
                else {                    
                    ngModel.$setViewValue(null);
                    element.val("");
                }
            }
            
            ngModel.$formatters.push(function (value) {
                if (angular.isString(value)) {
                    return parseFloat(value);
                }
                return value;
            });
            
            //This fixes this angular issue: 
            //https://github.com/angular/angular.js/issues/2144
            // which doesn't actually validate the number input properly since the model only changes when a real number is entered
            // but the input box still allows non-numbers to be entered which do not validate (only via html5)
            
            if (typeof element.prop('validity') === 'undefined') {
                return;
            }

            element.bind('input', function (e) {
                var validity = element.prop('validity');
                scope.$apply(function () {
                    ngModel.$setValidity('number', !validity.badInput);
                });
            });

        }
    };
}
angular.module('umbraco.directives').directive("fixNumber", fixNumber);

/**
* @ngdoc directive
* @name umbraco.directives.directive:hexBgColor
* @restrict A
* @description Used to set a hex background color on an element, this will detect valid hex and when it is valid it will set the color, otherwise
* a color will not be set.
**/
function hexBgColor() {
    return {        
        restrict: "A",
        link: function (scope, element, attr, formCtrl) {

            var origColor = null;
            if (attr.hexBgOrig) {
                //set the orig based on the attribute if there is one
                origColor = attr.hexBgOrig;
            }
            
            attr.$observe("hexBgColor", function (newVal) {
                if (newVal) {
                    if (!origColor) {
                        //get the orig color before changing it
                        origColor = element.css("border-color");
                    }
                    //validate it
                    if (/^([0-9a-f]{3}|[0-9a-f]{6})$/i.test(newVal)) {
                        element.css("background-color", "#" + newVal);
                        return;
                    }
                }
                element.css("background-color", origColor);
            });

        }
    };
}
angular.module('umbraco.directives').directive("hexBgColor", hexBgColor);
/**
* @ngdoc directive
* @name umbraco.directives.directive:headline
**/
angular.module("umbraco.directives")
  .directive('hotkey', function ($window, keyboardService, $log) {

      return function (scope, el, attrs) {
          
          //support data binding
    
          var keyCombo = scope.$eval(attrs["hotkey"]);
          if (!keyCombo) {
              keyCombo = attrs["hotkey"];
          }

          keyboardService.bind(keyCombo, function() {
              var element = $(el);
              if(element.is("a,button,input[type='button'],input[type='submit']")){
                element.click();
              }else{
                element.focus();
              }
          });
          
      };
  });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbProperty
* @restrict E
**/
angular.module("umbraco.directives.html")
    .directive('umbControlGroup', function (localizationService) {
        return {
            scope: {
                label: "@label",
                description: "@",
                hideLabel: "@",
                alias: "@"
            },
            require: '?^form',
            transclude: true,
            restrict: 'E',
            replace: true,        
            templateUrl: 'views/directives/html/umb-control-group.html',
            link: function (scope, element, attr, formCtrl) {

                scope.formValid = function() {
                    if (formCtrl) {
                        return formCtrl.$valid;
                    }
                    //there is no form.
                    return true;
                };

                if (scope.label && scope.label[0] === "@") {
                    scope.labelstring = localizationService.localize(scope.label.substring(1));
                }
                else {
                    scope.labelstring = scope.label;
                }
            }
        };
    });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbProperty
* @restrict E
**/
angular.module("umbraco.directives.html")
    .directive('umbPane', function () {
        return {
            transclude: true,
            restrict: 'E',
            replace: true,        
            templateUrl: 'views/directives/html/umb-pane.html'
        };
    });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbPanel
* @restrict E
**/
angular.module("umbraco.directives.html")
	.directive('umbPanel', function($timeout){
		return {
			restrict: 'E',
			replace: true,
			transclude: 'true',
			templateUrl: 'views/directives/html/umb-panel.html',
			link: function (scope, el, attrs) {
				
				function _setClass(resize){
					var bar = $(".tab-content .active .umb-tab-buttons");

					//incase this runs without any tabs
					if(bar.length === 0){
						bar = $(".tab-content .umb-tab-buttons");
					}

					//no need to process
					if(resize){
						bar.removeClass("umb-bottom-bar");
					}	

					//already positioned
					if(bar.hasClass("umb-bottom-bar")){
						return;
					}

					var offset = bar.offset();

					if(offset){
						var bottom = bar.offset().top + bar.height();
			            if(bottom > $(window).height()){
							bar.addClass("umb-bottom-bar");
							$(".tab-content .active").addClass("with-buttons");
						}else{
							bar.removeClass("umb-bottom-bar");
							$(".tab-content .active").removeClass("with-buttons");
						}	
					}
				}


				//initial loading
				$timeout(function(){
					$('a[data-toggle="tab"]').on('shown', function (e) {
						_setClass();	
					});
					_setClass();
				}, 1000, false);
				
				
				$(window).bind("resize", function () {
				  _setClass(true);
				});	
			}
		};
	});
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbPhotoFolder
* @restrict E
**/
angular.module("umbraco.directives.html")
.directive('umbPhotoFolder', function ($compile, $log, $timeout, $filter, imageHelper, umbRequestHelper) {

    function renderCollection(scope, photos) {
        // get row width - this is fixed.
        var w = scope.lastWidth;
        var rows = [];

        // initial height - effectively the maximum height +/- 10%;
        var h = Math.max(scope.minHeight, Math.floor(w / 5));

        // store relative widths of all images (scaled to match estimate height above)
        var ws = [];
        $.each(photos, function (key, val) {

            val.width_n = $.grep(val.properties, function (v, index) { return (v.alias === "umbracoWidth"); })[0];
            val.height_n = $.grep(val.properties, function (v, index) { return (v.alias === "umbracoHeight"); })[0];

            //val.url_n = imageHelper.getThumbnail({ imageModel: val, scope: scope });

            if (val.width_n && val.height_n) {
                var wt = parseInt(val.width_n.value, 10);
                var ht = parseInt(val.height_n.value, 10);

                if (ht !== h) {
                    wt = Math.floor(wt * (h / ht));
                }

                ws.push(wt);
            } else {
                //if its files or folders, we make them square
                ws.push(scope.minHeight);
            }
        });


        var rowNum = 0;
        var limit = photos.length;
        while (scope.baseline < limit) {
            rowNum++;
            // number of images appearing in this row
            var c = 0;
            // total width of images in this row - including margins
            var tw = 0;

            // calculate width of images and number of images to view in this row.
            while ((tw * 1.1 < w) && (scope.baseline + c < limit)) {
                tw += ws[scope.baseline + c++] + scope.border * 2;
            }

            // Ratio of actual width of row to total width of images to be used.
            var r = w / tw;
            // image number being processed
            var i = 0;
            // reset total width to be total width of processed images
            tw = 0;

            // new height is not original height * ratio
            var ht = Math.floor(h * r);

            var row = {};
            row.photos = [];
            row.style = {};
            row.style = { "height": ht + scope.border * 2, "width": scope.lastWidth };
            rows.push(row);

            while (i < c) {
                var photo = photos[scope.baseline + i];
                // Calculate new width based on ratio
                var wt = Math.floor(ws[scope.baseline + i] * r);
                // add to total width with margins
                tw += wt + scope.border * 2;

                //get the image property (if one exists)
                var imageProp = imageHelper.getImagePropertyValue({ imageModel: photo });
                if (!imageProp) {
                    //TODO: Do something better than this!!
                    photo.thumbnail = "none";
                }
                else {
                    
                    //get the proxy url for big thumbnails (this ensures one is always generated)
                    var thumbnailUrl = umbRequestHelper.getApiUrl(
                        "mediaApiBaseUrl",
                        "GetBigThumbnail",
                        [{ mediaId: photo.id }]);
                    photo.thumbnail = thumbnailUrl;
                }
                
                photo.style = { "width": wt, "height": ht, "margin": scope.border + "px", "cursor": "pointer" };
                row.photos.push(photo);
                i++;
            }

            // set row height to actual height + margins
            scope.baseline += c;

            // if total width is slightly smaller than 
            // actual div width then add 1 to each 
            // photo width till they match
            
            /*i = 0;
            while (tw < w-1) {
                row.photos[i].style.width++;
                i = (i + 1) % c;
                tw++;
            }*/

            // if total width is slightly bigger than 
            // actual div width then subtract 1 from each 
            // photo width till they match
            i = 0;
            while (tw > w-1) {
                row.photos[i].style.width--;
                i = (i + 1) % c;
                tw--;
            }
        }

        return rows;
    }
    
    return {
        restrict: 'E',
        replace: true,
        require: '?ngModel',
        terminate: true,
        templateUrl: 'views/directives/html/umb-photo-folder.html',
        link: function (scope, element, attrs, ngModel) {
            
            ngModel.$render = function () {
                if (ngModel.$modelValue) {

                    $timeout(function () {
                        var photos = ngModel.$modelValue;

                        scope.imagesOnly = element.attr('imagesOnly');
                        scope.baseline = element.attr('baseline') ? parseInt(element.attr('baseline'), 10) : 0;
                        scope.minWidth = element.attr('min-width') ? parseInt(element.attr('min-width'), 10) : 420;
                        scope.minHeight = element.attr('min-height') ? parseInt(element.attr('min-height'), 10) : 200;
                        scope.border = element.attr('border') ? parseInt(element.attr('border'), 10) : 5;
                        scope.clickHandler = scope.$eval(element.attr('on-click'));
                        scope.lastWidth = Math.max(element.width(), scope.minWidth);

                        scope.rows = renderCollection(scope, photos);

                        if (attrs.filterBy) {
                            scope.$watch(attrs.filterBy, function (newVal, oldVal) {
                                if (newVal !== oldVal) {
                                    var p = $filter('filter')(photos, newVal, false);
                                    scope.baseline = 0;
                                    var m = renderCollection(scope, p);
                                    scope.rows = m;
                                }
                            });
                        }

                    }, 500); //end timeout
                } //end if modelValue

            }; //end $render
        }
    };
});

/**
* @ngdoc directive
* @name umbraco.directives.directive:umbPanel
* @restrict E
**/
angular.module("umbraco.directives.html")
	.directive('umbUploadDropzone', function(){
		return {
			restrict: 'E',
			replace: true,
			scope: {
				dropping: "=",
				files: "="
			},
			transclude: 'true',
			templateUrl: 'views/directives/html/umb-upload-dropzone.html'
		};
	});
angular.module("umbraco.directives")
.directive('localize', function ($log, localizationService) {
    return {
        restrict: 'E',
        scope:{
            key: '@'
        },
        replace: true,
        link: function (scope, element, attrs) {
            var key = scope.key;
            localizationService.localize(key).then(function(value){
                element.html(value);
            });
        }
    };
})
.directive('localize', function ($log, localizationService) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var keys = attrs.localize.split(',');

            angular.forEach(keys, function(value, key){
                var attr = element.attr(value);
                if(attr){
                    if(attr[0] === '@'){
                        var t = localizationService.tokenize(attr.substring(1), scope);
                        localizationService.localize(t.key, t.tokens).then(function(val){
                                element.attr(value, val);
                        });
                    }
                }
            });

        }
    };
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:preventDefault
**/
angular.module("umbraco.directives")
    .directive('preventDefault', function() {
        return function(scope, element, attrs) {

            var enabled = true;
            //check if there's a value for the attribute, if there is and it's false then we conditionally don't 
            //prevent default.
            if (attrs.preventDefault) {
                attrs.$observe("preventDefault", function (newVal) {
                    enabled = (newVal === "false" || newVal === 0 || newVal === false) ? false : true;
                });
            }

            $(element).click(function (event) {
                if (event.metaKey || event.ctrlKey) {
                    return;
                }
                else {
                    if (enabled === true) {
                        event.preventDefault();
                    }
                }
            });
        };
    });
/**
 * @ngdoc directive
 * @name umbraco.directives.directive:resizeToContent
 * @element div
 * @function
 *
 * @description
 * Resize iframe's automatically to fit to the content they contain
 *
 * @example
   <example module="umbraco.directives">
     <file name="index.html">
         <iframe resize-to-content src="meh.html"></iframe>
     </file>
   </example>
 */
angular.module("umbraco.directives")
  .directive('resizeToContent', function ($window, $timeout) {
    return function (scope, el, attrs) {
       var iframe = el[0];
       var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
       if (iframeWin.document.body) {

          $timeout(function(){
              var height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
              el.height(height);
          }, 3000);
       }
    };
  });
angular.module("umbraco.directives")
.directive('sectionIcon', function ($compile, iconHelper) {
    return {
        restrict: 'E',
        replace: true,

        link: function (scope, element, attrs) {

            var icon = attrs.icon;

            if (iconHelper.isLegacyIcon(icon)) {
                //its a known legacy icon, convert to a new one
                element.html("<i class='" + iconHelper.convertFromLegacyIcon(icon) + "'></i>");
            }
            else if (iconHelper.isFileBasedIcon(icon)) {
                var convert = iconHelper.convertFromLegacyImage(icon);
                if(convert){
                    element.html("<i class='icon-section " + convert + "'></i>");
                }else{
                    element.html("<img src='images/tray/" + icon + "'>");
                }
                //it's a file, normally legacy so look in the icon tray images
            }
            else {
                //it's normal
                element.html("<i class='icon-section " + icon + "'></i>");
            }
        }
    };
});
angular.module("umbraco.directives")
  .directive('selectOnFocus', function () {
    return function (scope, el, attrs) {
        $(el).bind("click", function(){
          this.select();
        });
    };
  });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbItemSorter
* @function
* @element ANY
* @restrict E
* @description A re-usable directive for sorting items
**/
function umbItemSorter(angularHelper) {
    return {
        scope: {
            model: "="
        },
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-item-sorter.html',
        link: function(scope, element, attrs, ctrl) {
            var defaultModel = {
                okButton: "Ok",
                successMsg: "Sorting successful",
                complete: false
            };
            //assign user vals to default
            angular.extend(defaultModel, scope.model);
            //re-assign merged to user
            scope.model = defaultModel;

            scope.performSort = function() {
                scope.$emit("umbItemSorter.sorting", {
                    sortedItems: scope.model.itemsToSort
                });
            };

            scope.handleCancel = function () {
                scope.$emit("umbItemSorter.cancel");
            };

            scope.handleOk = function() {
                scope.$emit("umbItemSorter.ok");
            };
            
            //defines the options for the jquery sortable
            scope.sortableOptions = {
                axis: 'y',
                cursor: "move",
                placeholder: "ui-sortable-placeholder",
                update: function (ev, ui) {
                    //highlight the item when the position is changed
                    $(ui.item).effect("highlight", { color: "#049cdb" }, 500);
                },
                stop: function (ev, ui) {
                    //the ui-sortable directive already ensures that our list is re-sorted, so now we just
                    // need to update the sortOrder to the index of each item
                    angularHelper.safeApply(scope, function () {
                        angular.forEach(scope.itemsToSort, function (val, index) {
                            val.sortOrder = index + 1;
                        });

                    });
                }
            };
        }
    };
}

angular.module('umbraco.directives').directive("umbItemSorter", umbItemSorter);

/**
* @ngdoc directive
* @name umbraco.directives.directive:umbAvatar
* @restrict E
**/
function avatarDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-avatar.html',
        scope: {
            name: '@',
            email: '@',
            hash: '@'
        },
        link: function(scope, element, attr, ctrl) {

            scope.$watch("hash", function (val) {
                //set the gravatar url
                scope.gravatar = "http://www.gravatar.com/avatar/" + val + "?s=40";
            });
            
        }
    };
}

angular.module('umbraco.directives').directive("umbAvatar", avatarDirective);

/**
 * @ngdoc directive
 * @name umbraco.directives.directive:umbConfirm
 * @function
 * @description
 * A confirmation dialog
 * 
 * @restrict E
 */
function confirmDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-confirm.html',
        scope: {
            onConfirm: '=',
            onCancel: '=',
            caption: '@'
        },
        link: function (scope, element, attr, ctrl) {
            
        }
    };
}
angular.module('umbraco.directives').directive("umbConfirm", confirmDirective);
angular.module("umbraco.directives")
.directive('umbContextMenu', function (navigationService) {
    return {
        scope: {
            menuDialogTitle: "@",
            currentSection: "@",
            currentNode: "=",
            menuActions: "="
        },
        restrict: 'E',
        replace: true,
        templateUrl: 'views/directives/umb-contextmenu.html',
        link: function (scope, element, attrs, ctrl) {
            
            //adds a handler to the context menu item click, we need to handle this differently
            //depending on what the menu item is supposed to do.
            scope.executeMenuItem = function (action) {
                navigationService.executeMenuAction(action, scope.currentNode, scope.currentSection);
            };
        }
    };
});
/**
* @ngdoc directive
* @function
* @name umbraco.directives.directive:umbEditor 
* @requires formController
* @restrict E
**/
angular.module("umbraco.directives")
    .directive('umbEditor', function (umbPropEditorHelper) {
        return {
            scope: {
                model: "=",
                isPreValue: "@"
            },
            require: "^form",
            restrict: 'E',
            replace: true,      
            templateUrl: 'views/directives/umb-editor.html',
            link: function (scope, element, attrs, ctrl) {

                //we need to copy the form controller val to our isolated scope so that
                //it get's carried down to the child scopes of this!
                //we'll also maintain the current form name.
                scope[ctrl.$name] = ctrl;

                if(!scope.model.alias){
                   scope.model.alias = Math.random().toString(36).slice(2);
                }

                scope.propertyEditorView = umbPropEditorHelper.getViewPath(scope.model.view, scope.isPreValue);
            }
        };
    });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbFileUpload
* @function
* @restrict A
* @scope
* @description
*  Listens for file input control changes and emits events when files are selected for use in other controllers.
**/
function umbFileUpload() {
    return {
        restrict: "A",
        scope: true,        //create a new scope
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //emit event upward
                scope.$emit("filesSelected", { files: files });                           
            });
        }
    };
}

angular.module('umbraco.directives').directive("umbFileUpload", umbFileUpload);
angular.module("umbraco.directives")
.directive('umbHeader', function($parse, $timeout){
    return {
        restrict: 'E',
        replace: true,
        transclude: 'true',
        templateUrl: 'views/directives/umb-header.html',
        //create a new isolated scope assigning a tabs property from the attribute 'tabs'
        //which is bound to the parent scope property passed in
        scope: {
            tabs: "="
        },
        link: function (scope, iElement, iAttrs) {

            var maxTabs = 4;

            function collectFromDom(activeTab){
                var $panes = $('div.tab-content');
                
                angular.forEach($panes.find('.tab-pane'), function (pane, index) {
                    var $this = angular.element(pane);

                    var id = $this.attr("rel");
                    var label = $this.attr("label");
                    var tab = {id: id, label: label, active: false};
                    if(!activeTab){
                        tab.active = true;
                        activeTab = tab;
                    }

                    if ($this.attr("rel") === String(activeTab.id)) {
                        $this.addClass('active');
                    }
                    else {
                        $this.removeClass('active');
                    }
                    
                    if(label){
                            scope.visibleTabs.push(tab);
                    }

                });
            }

            scope.showTabs = iAttrs.tabs ? true : false;
            scope.visibleTabs = [];
            scope.overflownTabs = [];

            $timeout(function () {
                collectFromDom(undefined);
            }, 500);

            //when the tabs change, we need to hack the planet a bit and force the first tab content to be active,
            //unfortunately twitter bootstrap tabs is not playing perfectly with angular.
            scope.$watch("tabs", function (newValue, oldValue) {

                angular.forEach(newValue, function(val, index){
                        var tab = {id: val.id, label: val.label};
                        scope.visibleTabs.push(tab);
                });
                
                //don't process if we cannot or have already done so
                if (!newValue) {return;}
                if (!newValue.length || newValue.length === 0){return;}
                
                var activeTab = _.find(newValue, function (item) {
                    return item.active;
                });

                //we need to do a timeout here so that the current sync operation can complete
                // and update the UI, then this will fire and the UI elements will be available.
                $timeout(function () {
                    collectFromDom(activeTab);
                }, 500);
                
            });
        }
    };
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:login
* @function
* @element ANY
* @restrict E
**/
function loginDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-login.html'        
    };
}

angular.module('umbraco.directives').directive("umbLogin", loginDirective);

/**
* @ngdoc directive
* @name umbraco.directives.directive:umbNavigation
* @restrict E
**/
function leftColumnDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-navigation.html'
    };
}

angular.module('umbraco.directives').directive("umbNavigation", leftColumnDirective);

/**
 * @ngdoc directive
 * @name umbraco.directives.directive:umbNotifications
 */
function notificationDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-notifications.html'
    };
}

angular.module('umbraco.directives').directive("umbNotifications", notificationDirective);
angular.module("umbraco.directives")
.directive('umbOptionsMenu', function ($injector, treeService, navigationService, umbModelMapper, appState) {
    return {
        scope: {
            currentSection: "@",
            currentNode: "="
        },
        restrict: 'E',
        replace: true,
        templateUrl: 'views/directives/umb-optionsmenu.html',
        link: function (scope, element, attrs, ctrl) {

            //adds a handler to the context menu item click, we need to handle this differently
            //depending on what the menu item is supposed to do.
            scope.executeMenuItem = function (action) {
                navigationService.executeMenuAction(action, scope.currentNode, scope.currentSection);
            };

            //callback method to go and get the options async
            scope.getOptions = function () {

                if (!scope.currentNode) {
                    return;
                }
                
                //when the options item is selected, we need to set the current menu item in appState (since this is synonymous with a menu)
                appState.setMenuState("currentNode", scope.currentNode);

                if (!scope.actions) {
                    treeService.getMenu({ treeNode: scope.currentNode })
                        .then(function (data) {
                            scope.actions = data.menuItems;
                        });
                }
            };

        }
    };
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbProperty
* @restrict E
**/
angular.module("umbraco.directives")
    .directive('umbProperty', function (umbPropEditorHelper) {
        return {
            scope: {
                property: "="
            },
            transclude: true,
            restrict: 'E',
            replace: true,        
            templateUrl: 'views/directives/umb-property.html',
            link: function (scope, element, attrs, ctrl) {

            }
        };
    });
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbSections
* @restrict E
**/
function sectionsDirective($timeout, $window, navigationService, treeService, sectionResource, appState) {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/directives/umb-sections.html',
        link: function (scope, element, attr, ctrl) {
			
            //setup scope vars
			scope.maxSections = 7;
			scope.overflowingSections = 0;
            scope.sections = [];
            scope.currentSection = appState.getSectionState("currentSection");
            scope.showTray = false; //appState.getGlobalState("showTray");
            scope.stickyNavigation = appState.getGlobalState("stickyNavigation");
            scope.needTray = false;
            scope.trayAnimation = function() {                
                if (scope.showTray) {
                    return 'slide';
                }
                else {
                    return '';
                }
            };

			function loadSections(){
				sectionResource.getSections()
					.then(function (result) {
						scope.sections = result;
						calculateHeight();
					});
			}

			function calculateHeight(){
				$timeout(function(){
					//total height minus room for avatar and help icon
					var height = $(window).height()-200;
					scope.totalSections = scope.sections.length;
					scope.maxSections = Math.floor(height / 70);
					scope.needTray = false;

					if(scope.totalSections > scope.maxSections){
						scope.needTray = true;
						scope.overflowingSections = scope.maxSections - scope.totalSections;
					}
				});
			}
            
            //Listen for global state changes
			scope.$on("appState.globalState.changed", function (e, args) {
			    if (args.key === "showTray") {
			        scope.showTray = args.value;
			    }
			    if (args.key === "stickyNavigation") {
			        scope.stickyNavigation = args.value;
			    }
			});

			scope.$on("appState.sectionState.changed", function (e, args) {
			    if (args.key === "currentSection") {
			        scope.currentSection = args.value;
			    }
			});

			//on page resize
			window.onresize = calculateHeight;
			
			scope.avatarClick = function(){
				navigationService.showUserDialog();
			};

			scope.helpClick = function(){
				navigationService.showHelpDialog();
			};

			scope.sectionClick = function (section) {
			    navigationService.hideSearch();
				navigationService.showTree(section.alias);
			};

			scope.sectionDblClick = function(section){
				navigationService.reloadSection(section.alias);
			};

			scope.trayClick = function(){
				navigationService.showTray();
			};
            
			loadSections();

        }
    };
}

angular.module('umbraco.directives').directive("umbSections", sectionsDirective);

/**
* @ngdoc directive
* @name umbraco.directives.directive:umbFileUpload
* @function
* @restrict A
* @scope
* @description
*  A single file upload field that will reset itself based on the object passed in for the rebuild parameter. This
*  is required because the only way to reset an upload control is to replace it's html.
**/
function umbSingleFileUpload($compile) {
    return {
        restrict: "E",
        scope: {
            rebuild: "="
        },
        replace: true,
        template: "<div><input type='file' umb-file-upload /></div>",
        link: function (scope, el, attrs) {

            scope.$watch("rebuild", function (newVal, oldVal) {
                if (newVal && newVal !== oldVal) {
                    //recompile it!
                    el.html("<input type='file' umb-file-upload />");
                    $compile(el.contents())(scope);
                }
            });

        }
    };
}

angular.module('umbraco.directives').directive("umbSingleFileUpload", umbSingleFileUpload);
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbTab 
* @restrict E
**/
angular.module("umbraco.directives")
.directive('umbTab', function(){
	return {
		restrict: 'E',
		replace: true,
		transclude: 'true',
		templateUrl: 'views/directives/umb-tab.html'
	};
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbTabView 
* @restrict E
**/
angular.module("umbraco.directives")
.directive('umbTabView', function($timeout, $log){
	return {
		restrict: 'E',
		replace: true,
		transclude: 'true',
		templateUrl: 'views/directives/umb-tab-view.html'
	};
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:umbTree
* @restrict E
**/
function umbTreeDirective($compile, $log, $q, $rootScope, treeService, notificationsService, $timeout) {

    return {
        restrict: 'E',
        replace: true,
        terminal: false,

        scope: {
            section: '@',
            treealias: '@',
            hideoptions: '@',
            hideheader: '@',
            cachekey: '@',
            isdialog: '@',
            //Custom query string arguments to pass in to the tree as a string, example: "startnodeid=123&something=value"
            customtreeparams: '@',
            eventhandler: '='
        },

        compile: function(element, attrs) {
            //config
            //var showheader = (attrs.showheader !== 'false');
            var hideoptions = (attrs.hideoptions === 'true') ? "hide-options" : "";
            var template = '<ul class="umb-tree ' + hideoptions + '"><li class="root">';
            template += '<div ng-hide="hideheader">' +
                '<h5><a href="#/{{section}}" ng-click="select(this, tree.root, $event)" on-right-click="altSelect(this, tree.root, $event)"  class="root-link">{{tree.name}}</a></h5>' +
                '<a href class="umb-options" ng-hide="tree.root.isContainer || !tree.root.menuUrl" ng-click="options(this, tree.root, $event)" ng-swipe-right="options(this, tree.root, $event)"><i></i><i></i><i></i></a>' +
                '</div>';
            template += '<ul>' +
                '<umb-tree-item ng-repeat="child in tree.root.children" eventhandler="eventhandler" node="child" current-node="currentNode" tree="child" section="{{section}}" ng-animate="animation()"></umb-tree-item>' +
                '</ul>' +
                '</li>' +
                '</ul>';

            element.replaceWith(template);

            return function(scope, elem, attr, controller) {

                //flag to track the last loaded section when the tree 'un-loads'. We use this to determine if we should
                // re-load the tree again. For example, if we hover over 'content' the content tree is shown. Then we hover
                // outside of the tree and the tree 'un-loads'. When we re-hover over 'content', we don't want to re-load the 
                // entire tree again since we already still have it in memory. Of course if the section is different we will
                // reload it. This saves a lot on processing if someone is navigating in and out of the same section many times
                // since it saves on data retreival and DOM processing.
                var lastSection = "";
                
                //setup a default internal handler
                if (!scope.eventhandler) {
                    scope.eventhandler = $({});
                }

                //flag to enable/disable delete animations
                var deleteAnimations = false;


                /** Helper function to emit tree events */
                function emitEvent(eventName, args) {
                    if (scope.eventhandler) {
                        $(scope.eventhandler).trigger(eventName, args);
                    }
                }
                
                /** This will deleteAnimations to true after the current digest */
                function enableDeleteAnimations() {
                    //do timeout so that it re-enables them after this digest
                    $timeout(function () {
                        //enable delete animations
                        deleteAnimations = true;
                    }, 0, false);
                }


                /*this is the only external interface a tree has */
                function setupExternalEvents() {
                    if (scope.eventhandler) {

                        scope.eventhandler.clearCache = function(section) {
                            treeService.clearCache({ section: section });
                        };

                        scope.eventhandler.load = function(section) {
                            scope.section = section;
                            loadTree();
                        };

                        scope.eventhandler.reloadNode = function(node) {

                            if (!node) {
                                node = scope.currentNode;
                            }

                            if (node) {
                                scope.loadChildren(node, true);
                            }
                        };
                        
                        /** 
                            Used to do the tree syncing. If the args.tree is not specified we are assuming it has been 
                            specified previously using the _setActiveTreeType
                        */
                        scope.eventhandler.syncTree = function(args) {
                            if (!args) {
                                throw "args cannot be null";
                            }
                            if (!args.path) {
                                throw "args.path cannot be null";
                            }
                            
                            var deferred = $q.defer();

                            //this is super complex but seems to be working in other places, here we're listening for our
                            // own events, once the tree is sycned we'll resolve our promise.
                            scope.eventhandler.one("treeSynced", function (e, syncArgs) {
                                deferred.resolve(syncArgs);
                            });

                            //this should normally be set unless it is being called from legacy 
                            // code, so set the active tree type before proceeding.
                            if (args.tree) {
                                loadActiveTree(args.tree);
                            }

                            if (angular.isString(args.path)) {
                                args.path = args.path.replace('"', '').split(',');
                            }

                            //reset current node selection
                            //scope.currentNode = null;

                            //filter the path for root node ids
                            args.path = _.filter(args.path, function (item) { return (item !== "init" && item !== "-1"); });
                            loadPath(args.path, args.forceReload, args.activate);

                            return deferred.promise;
                        };

                        /** 
                            Internal method that should ONLY be used by the legacy API wrapper, the legacy API used to 
                            have to set an active tree and then sync, the new API does this in one method by using syncTree.
                            loadChildren is optional but if it is set, it will set the current active tree and load the root
                            node's children - this is synonymous with the legacy refreshTree method - again should not be used
                            and should only be used for the legacy code to work.
                        */
                        scope.eventhandler._setActiveTreeType = function(treeAlias, loadChildren) {
                            loadActiveTree(treeAlias, loadChildren);
                        };
                    }
                }


                //helper to load a specific path on the active tree as soon as its ready
                function loadPath(path, forceReload, activate) {

                    if (scope.activeTree) {
                        syncTree(scope.activeTree, path, forceReload, activate);
                    }
                    else {
                        scope.eventhandler.one("activeTreeLoaded", function (e, args) {
                            syncTree(args.tree, path, forceReload, activate);
                        });
                    }
                }

                
                //given a tree alias, this will search the current section tree for the specified tree alias and
                //set that to the activeTree
                //NOTE: loadChildren is ONLY used for legacy purposes, do not use this when syncing the tree as it will cause problems
                // since there will be double request and event handling operations.
                function loadActiveTree(treeAlias, loadChildren) {
                    scope.activeTree = undefined;

                    function doLoad(tree) {
                        var childrenAndSelf = [tree].concat(tree.children);
                        scope.activeTree = _.find(childrenAndSelf, function (node) {
                             return node.metaData.treeAlias === treeAlias;
                        });
                        
                        if (!scope.activeTree) {
                            throw "Could not find the tree " + treeAlias + ", activeTree has not been set";
                        }

                        //This is only used for the legacy tree method refreshTree!
                        if (loadChildren) {
                            scope.activeTree.expanded = true;
                            scope.loadChildren(scope.activeTree, false).then(function() {
                                emitEvent("activeTreeLoaded", { tree: scope.activeTree });
                            });
                        }
                        else {
                            emitEvent("activeTreeLoaded", { tree: scope.activeTree });
                        }
                    }

                    if (scope.tree) {
                        doLoad(scope.tree.root);
                    }
                    else {
                        scope.eventhandler.one("treeLoaded", function(e, args) {
                            doLoad(args.tree);
                        });
                    }
                }


                /** Method to load in the tree data */

                function loadTree() {
                    if (!scope.loading && scope.section) {
                        scope.loading = true;

                        //anytime we want to load the tree we need to disable the delete animations
                        deleteAnimations = false;

                        //default args
                        var args = { section: scope.section, tree: scope.treealias, cacheKey: scope.cachekey, isDialog: scope.isdialog ? scope.isdialog : false };

                        //add the extra query string params if specified
                        if (scope.customtreeparams) {
                            args["queryString"] = scope.customtreeparams;
                        }

                        treeService.getTree(args)
                            .then(function(data) {
                                //set the data once we have it
                                scope.tree = data;
                                
                                enableDeleteAnimations();

                                scope.loading = false;

                                //set the root as the current active tree
                                scope.activeTree = scope.tree.root;
                                emitEvent("treeLoaded", { tree: scope.tree.root });

                            }, function(reason) {
                                scope.loading = false;
                                notificationsService.error("Tree Error", reason);
                            });
                    }
                }

                /** syncs the tree, the treeNode can be ANY tree node in the tree that requires syncing */
                function syncTree(treeNode, path, forceReload, activate) {

                    deleteAnimations = false;

                    treeService.syncTree({
                        node: treeNode,
                        path: path,
                        forceReload: forceReload
                    }).then(function (data) {

                        if (activate === undefined || activate === true) {
                            scope.currentNode = data;
                        }
                        
                        emitEvent("treeSynced", { node: data, activate: activate });
                        
                        enableDeleteAnimations();
                    });

                }

                /** method to set the current animation for the node. 
                 *  This changes dynamically based on if we are changing sections or just loading normal tree data. 
                 *  When changing sections we don't want all of the tree-ndoes to do their 'leave' animations.
                 */
                scope.animation = function() {
                    if (deleteAnimations && scope.tree && scope.tree.root && scope.tree.root.expanded) {
                        return { leave: 'tree-node-delete-leave' };
                    }
                    else {
                        return {};
                    }
                };

                /* helper to force reloading children of a tree node */
                scope.loadChildren = function(node, forceReload) {
                    var deferred = $q.defer();

                    //emit treeNodeExpanding event, if a callback object is set on the tree
                    emitEvent("treeNodeExpanding", { tree: scope.tree, node: node });

                    //standardising                
                    if (!node.children) {
                        node.children = [];
                    }

                    if (forceReload || (node.hasChildren && node.children.length === 0)) {
                        //get the children from the tree service
                        treeService.loadNodeChildren({ node: node, section: scope.section })
                            .then(function(data) {
                                //emit expanded event
                                emitEvent("treeNodeExpanded", { tree: scope.tree, node: node, children: data });
                                
                                enableDeleteAnimations();

                                deferred.resolve(data);
                            });
                    }
                    else {
                        emitEvent("treeNodeExpanded", { tree: scope.tree, node: node, children: node.children });
                        node.expanded = true;
                        
                        enableDeleteAnimations();

                        deferred.resolve(node.children);
                    }

                    return deferred.promise;
                };

                /**
                  Method called when the options button next to the root node is called.
                  The tree doesnt know about this, so it raises an event to tell the parent controller
                  about it.
                */
                scope.options = function(e, n, ev) {
                    emitEvent("treeOptionsClick", { element: e, node: n, event: ev });
                };

                /**
                  Method called when an item is clicked in the tree, this passes the 
                  DOM element, the tree node object and the original click
                  and emits it as a treeNodeSelect element if there is a callback object
                  defined on the tree
                */
                scope.select = function (e, n, ev) {
                    //on tree select we need to remove the current node - 
                    // whoever handles this will need to make sure the correct node is selected
                    //reset current node selection
                    scope.currentNode = null;

                    emitEvent("treeNodeSelect", { element: e, node: n, event: ev });
                };

                scope.altSelect = function(e, n, ev) {
                    emitEvent("treeNodeAltSelect", { element: e, tree: scope.tree, node: n, event: ev });
                };
                
                //watch for section changes
                scope.$watch("section", function(newVal, oldVal) {

                    if (!scope.tree) {
                        loadTree();
                    }

                    if (!newVal) {
                        //store the last section loaded
                        lastSection = oldVal;
                    }
                    else if (newVal !== oldVal && newVal !== lastSection) {
                        //only reload the tree data and Dom if the newval is different from the old one
                        // and if the last section loaded is different from the requested one.
                        loadTree();

                        //store the new section to be loaded as the last section
                        //clear any active trees to reset lookups
                        lastSection = newVal;
                    }
                });
                
                setupExternalEvents();
                loadTree();
            };
        }
    };
}

angular.module("umbraco.directives").directive('umbTree', umbTreeDirective);
/**
 * @ngdoc directive
 * @name umbraco.directives.directive:umbTreeItem
 * @element li
 * @function
 *
 * @description
 * Renders a list item, representing a single node in the tree.
 * Includes element to toggle children, and a menu toggling button
 *
 * **note:** This directive is only used internally in the umbTree directive
 *
 * @example
   <example module="umbraco">
    <file name="index.html">
         <umb-tree-item ng-repeat="child in tree.children" node="child" callback="callback" section="content"></umb-tree-item>
    </file>
   </example>
 */
angular.module("umbraco.directives")
.directive('umbTreeItem', function ($compile, $http, $templateCache, $interpolate, $log, $location, $rootScope, $window, treeService, $timeout) {
  return {
    restrict: 'E',
    replace: true,

    scope: {
      section: '@',
      cachekey: '@',
      eventhandler: '=',
      currentNode:'=',
      node:'=',
      tree:'='
    },

    template: '<li ng-class="{\'current\': (node == currentNode)}" on-right-click="altSelect(this, node, $event)"><div ng-style="setTreePadding(node)" ng-class="node.stateCssClass" ng-class="{\'loading\': node.loading}" ng-swipe-right="options(this, node, $event)" >' +
        '<ins ng-hide="node.hasChildren" style="width:18px;"></ins>' +        
        '<ins ng-show="node.hasChildren" ng-class="{\'icon-navigation-right\': !node.expanded, \'icon-navigation-down\': node.expanded}" ng-click="load(node)"></ins>' +
        '<i title="#{{node.routePath}}" class="{{node.cssClass}}"></i>' +
        '<a href ng-click="select(this, node, $event)" on-right-click="altSelect(this, node, $event)">{{node.name}}</a>' +
        '<a href class="umb-options" ng-hide="!node.menuUrl" ng-click="options(this, node, $event)"><i></i><i></i><i></i></a>' +
        '<div ng-show="node.loading" class="l"><div></div></div>' +
        '</div>' +
        '</li>',

    link: function (scope, element, attrs) {
        
        //flag to enable/disable delete animations, default for an item is tru
        var deleteAnimations = true;

        /** Helper function to emit tree events */
        function emitEvent(eventName, args) {
          if(scope.eventhandler){
            $(scope.eventhandler).trigger(eventName,args);
          }
        }

        /** updates the node's styles */
        function styleNode(node) {
            node.stateCssClass = (node.cssClasses || []).join(" ");

            if (node.style) {
                $(element).find("i").attr("style", node.style);
            }
        }

        /** This will deleteAnimations to true after the current digest */
        function enableDeleteAnimations() {
            //do timeout so that it re-enables them after this digest
            $timeout(function () {
                //enable delete animations
                deleteAnimations = true;
            }, 0, false);
        }

        //add a method to the node which we can use to call to update the node data if we need to ,
        // this is done by sync tree, we don't want to add a $watch for each node as that would be crazy insane slow
        // so we have to do this
        scope.node.updateNodeData = function (newNode) {            
            _.extend(scope.node, newNode);
            //now update the styles
            styleNode(scope.node);
        };

        /**
          Method called when the options button next to a node is called
          In the main tree this opens the menu, but internally the tree doesnt
          know about this, so it simply raises an event to tell the parent controller
          about it.
        */
        scope.options = function(e, n, ev){ 
          emitEvent("treeOptionsClick", {element: e, tree: scope.tree, node: n, event: ev});
        };

        /**
          Method called when an item is clicked in the tree, this passes the 
          DOM element, the tree node object and the original click
          and emits it as a treeNodeSelect element if there is a callback object
          defined on the tree
        */
        scope.select = function(e,n,ev){
            emitEvent("treeNodeSelect", { element: e, tree: scope.tree, node: n, event: ev });
        };

        /**
          Method called when an item is right-clicked in the tree, this passes the 
          DOM element, the tree node object and the original click
          and emits it as a treeNodeSelect element if there is a callback object
          defined on the tree
        */
        scope.altSelect = function(e,n,ev){
            emitEvent("treeNodeAltSelect", { element: e, tree: scope.tree, node: n, event: ev });
        };

        /** method to set the current animation for the node. 
        *  This changes dynamically based on if we are changing sections or just loading normal tree data. 
        *  When changing sections we don't want all of the tree-ndoes to do their 'leave' animations.
        */
        scope.animation = function () {
            if (deleteAnimations && scope.node.expanded) {
                return { leave: 'tree-node-delete-leave' };
            }
            else {
                return {};
            }            
        };

        /**
          Method called when a node in the tree is expanded, when clicking the arrow
          takes the arrow DOM element and node data as parameters
          emits treeNodeCollapsing event if already expanded and treeNodeExpanding if collapsed
        */
        scope.load = function(node) {
            if (node.expanded) {
                deleteAnimations = false;
                emitEvent("treeNodeCollapsing", {tree: scope.tree, node: node });
                node.expanded = false;
            }
            else {
                scope.loadChildren(node, false);
            }
        };

        /* helper to force reloading children of a tree node */
        scope.loadChildren = function(node, forceReload){
            //emit treeNodeExpanding event, if a callback object is set on the tree
            emitEvent("treeNodeExpanding", { tree: scope.tree, node: node });
            
            if (node.hasChildren && (forceReload || !node.children || (angular.isArray(node.children) && node.children.length === 0))) {
                //get the children from the tree service
                treeService.loadNodeChildren({ node: node, section: scope.section })
                    .then(function(data) {
                        //emit expanded event
                        emitEvent("treeNodeExpanded", {tree: scope.tree, node: node, children: data });
                        enableDeleteAnimations();
                    });
            }
            else {
                emitEvent("treeNodeExpanded", { tree: scope.tree, node: node, children: node.children });
                node.expanded = true;
                enableDeleteAnimations();
            }
        };

        /**
          Helper method for setting correct element padding on tree DOM elements
          Since elements are not children of eachother, we need this indenting done
          manually
        */
        scope.setTreePadding = function(node) {
          return { 'padding-left': (node.level * 20) + "px" };
        };

        //if the current path contains the node id, we will auto-expand the tree item children

        styleNode(scope.node);
        
        var template = '<ul ng-class="{collapsed: !node.expanded}"><umb-tree-item  ng-repeat="child in node.children" eventhandler="eventhandler" tree="tree" current-node="currentNode" node="child" section="{{section}}" ng-animate="animation()"></umb-tree-item></ul>';
        var newElement = angular.element(template);
        $compile(newElement)(scope);
        element.append(newElement);
    }
  };
});

/**
* @description Utillity directives for key and field events
**/
angular.module('umbraco.directives')

.directive('onKeyup', function () {
    return function (scope, elm, attrs) {
        elm.bind("keyup", function () {
            scope.$apply(attrs.onKeyup);
        });
    };
})

.directive('onKeydown', function () {
    return {
        link: function (scope, elm, attrs) {
            $key('keydown', scope, elm, attrs);
        }
    };
})

.directive('onBlur', function () {
    return function (scope, elm, attrs) {
        elm.bind("blur", function () {
            scope.$apply(attrs.onBlur);
        });
    };
})

.directive('onFocus', function () {
    return function (scope, elm, attrs) {
        elm.bind("focus", function () {
            scope.$apply(attrs.onFocus);
        });
    };
})

.directive('onRightClick',function(){
    
    document.oncontextmenu = function (e) {
       if(e.target.hasAttribute('on-right-click')) {
           e.preventDefault();
           e.stopPropagation(); 
           return false;
       }
    };  

    return function(scope,el,attrs){
        el.bind('contextmenu',function(e){
            e.preventDefault();
            e.stopPropagation();
            scope.$apply(attrs.onRightClick);
            return false;
        });
    };
});
/**
 * General-purpose validator for ngModel.
 * angular.js comes with several built-in validation mechanism for input fields (ngRequired, ngPattern etc.) but using
 * an arbitrary validation function requires creation of a custom formatters and / or parsers.
 * The ui-validate directive makes it easy to use any function(s) defined in scope as a validator function(s).
 * A validator function will trigger validation on both model and input changes.
 *
 * @example <input val-custom=" 'myValidatorFunction($value)' ">
 * @example <input val-custom="{ foo : '$value > anotherModel', bar : 'validateFoo($value)' }">
 * @example <input val-custom="{ foo : '$value > anotherModel' }" val-custom-watch=" 'anotherModel' ">
 * @example <input val-custom="{ foo : '$value > anotherModel', bar : 'validateFoo($value)' }" val-custom-watch=" { foo : 'anotherModel' } ">
 *
 * @param val-custom {string|object literal} If strings is passed it should be a scope's function to be used as a validator.
 * If an object literal is passed a key denotes a validation error key while a value should be a validator function.
 * In both cases validator function should take a value to validate as its argument and should return true/false indicating a validation result.
 */

 /* 
  This code comes from the angular UI project, we had to change the directive name and module
  but other then that its unmodified
 */
angular.module('umbraco.directives.validation')
.directive('valCustom', function () {

  return {
    restrict: 'A',
    require: 'ngModel',
    link: function (scope, elm, attrs, ctrl) {
      var validateFn, watch, validators = {},
        validateExpr = scope.$eval(attrs.valCustom);

      if (!validateExpr){ return;}

      if (angular.isString(validateExpr)) {
        validateExpr = { validator: validateExpr };
      }

      angular.forEach(validateExpr, function (exprssn, key) {
        validateFn = function (valueToValidate) {
          var expression = scope.$eval(exprssn, { '$value' : valueToValidate });
          if (angular.isObject(expression) && angular.isFunction(expression.then)) {
            // expression is a promise
            expression.then(function(){
              ctrl.$setValidity(key, true);
            }, function(){
              ctrl.$setValidity(key, false);
            });
            return valueToValidate;
          } else if (expression) {
            // expression is true
            ctrl.$setValidity(key, true);
            return valueToValidate;
          } else {
            // expression is false
            ctrl.$setValidity(key, false);
            return undefined;
          }
        };
        validators[key] = validateFn;
        ctrl.$formatters.push(validateFn);
        ctrl.$parsers.push(validateFn);
      });

      function apply_watch(watch)
      {
          //string - update all validators on expression change
          if (angular.isString(watch))
          {
              scope.$watch(watch, function(){
                  angular.forEach(validators, function(validatorFn){
                      validatorFn(ctrl.$modelValue);
                  });
              });
              return;
          }

          //array - update all validators on change of any expression
          if (angular.isArray(watch))
          {
              angular.forEach(watch, function(expression){
                  scope.$watch(expression, function()
                  {
                      angular.forEach(validators, function(validatorFn){
                          validatorFn(ctrl.$modelValue);
                      });
                  });
              });
              return;
          }

          //object - update appropriate validator
          if (angular.isObject(watch))
          {
              angular.forEach(watch, function(expression, validatorKey)
              {
                  //value is string - look after one expression
                  if (angular.isString(expression))
                  {
                      scope.$watch(expression, function(){
                          validators[validatorKey](ctrl.$modelValue);
                      });
                  }

                  //value is array - look after all expressions in array
                  if (angular.isArray(expression))
                  {
                      angular.forEach(expression, function(intExpression)
                      {
                          scope.$watch(intExpression, function(){
                              validators[validatorKey](ctrl.$modelValue);
                          });
                      });
                  }
              });
          }
      }
      // Support for val-custom-watch
      if (attrs.valCustomWatch){
          apply_watch( scope.$eval(attrs.valCustomWatch) );
      }
    }
  };
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:valHighlight
* @restrict A
* @description Used on input fields when you want to signal that they are in error, this will highlight the item for 1 second
**/
function valHighlight($timeout) {
    return {
        restrict: "A",
        link: function (scope, element, attrs, ctrl) {
            
            scope.$watch(function() {
                return scope.$eval(attrs.valHighlight);
            }, function(newVal, oldVal) {
                if (newVal === true) {
                    element.addClass("highlight-error");
                    $timeout(function () {
                        //set the bound scope property to false
                        scope[attrs.valHighlight] = false;
                    }, 1000);
                }
                else {
                    element.removeClass("highlight-error");
                }
            });
   
        }
    };
}
angular.module('umbraco.directives').directive("valHighlight", valHighlight);
angular.module('umbraco.directives.validation')
	.directive('valCompare',function () {
	return {
	        require: "ngModel", 
	        link: function (scope, elem, attrs, ctrl) {
	            
	            //TODO: Pretty sure this should be done using a requires ^form in the directive declaration	            
	            var otherInput = elem.inheritedData("$formController")[attrs.valCompare];

	            ctrl.$parsers.push(function(value) {
	                if(value === otherInput.$viewValue) {
	                    ctrl.$setValidity("valCompare", true);
	                    return value;
	                }
	                ctrl.$setValidity("valCompare", false);
	            });

	            otherInput.$parsers.push(function(value) {
	                ctrl.$setValidity("valCompare", value === ctrl.$viewValue);
	                return value;
	            });
	        }
	};
});
/**
* @ngdoc directive
* @name umbraco.directives.directive:valFormManager
* @restrict A
* @require formController
* @description Used to broadcast an event to all elements inside this one to notify that form validation has 
* changed. If we don't use this that means you have to put a watch for each directive on a form's validation
* changing which would result in much higher processing. We need to actually watch the whole $error collection of a form
* because just watching $valid or $invalid doesn't acurrately trigger form validation changing.
* This also sets the show-validation (or a custom) css class on the element when the form is invalid - this lets
* us css target elements to be displayed when the form is submitting/submitted.
* Another thing this directive does is to ensure that any .control-group that contains form elements that are invalid will
* be marked with the 'error' css class. This ensures that labels included in that control group are styled correctly.
**/
function valFormManager(serverValidationManager) {
    return {
        require: "form",
        restrict: "A",
        link: function (scope, element, attr, formCtrl) {

            scope.$watch(function () {
                return formCtrl.$error;
            }, function (e) {
                scope.$broadcast("valStatusChanged", { form: formCtrl });
                
                //find all invalid elements' .control-group's and apply the error class
                var inError = element.find(".control-group .ng-invalid").closest(".control-group");
                inError.addClass("error");

                //find all control group's that have no error and ensure the class is removed
                var noInError = element.find(".control-group .ng-valid").closest(".control-group").not(inError);
                noInError.removeClass("error");

            }, true);
            
            var className = attr.valShowValidation ? attr.valShowValidation : "show-validation";
            var savingEventName = attr.savingEvent ? attr.savingEvent : "formSubmitting";
            var savedEvent = attr.savedEvent ? attr.savingEvent : "formSubmitted";

            //we should show validation if there are any msgs in the server validation collection
            if (serverValidationManager.items.length > 0) {
                element.addClass(className);
            }

            //listen for the forms saving event
            scope.$on(savingEventName, function (ev, args) {
                element.addClass(className);
            });

            //listen for the forms saved event
            scope.$on(savedEvent, function (ev, args) {
                element.removeClass(className);
            });
        }
    };
}
angular.module('umbraco.directives').directive("valFormManager", valFormManager);
/**
* @ngdoc directive
* @name umbraco.directives.directive:valPropertyMsg
* @restrict A
* @element textarea
* @requires formController
* @description This directive is used to control the display of the property level validation message.
* We will listen for server side validation changes
* and when an error is detected for this property we'll show the error message.
* In order for this directive to work, the valStatusChanged directive must be placed on the containing form.
**/
function valPropertyMsg(serverValidationManager) {    
    return {
        scope: {
            property: "=property"
        },
        require: "^form",   //require that this directive is contained within an ngForm
        replace: true,      //replace the element with the template
        restrict: "E",      //restrict to element
        template: "<div ng-show=\"errorMsg != ''\" class='alert alert-error property-error' >{{errorMsg}}</div>",        
       
        /**
            Our directive requries a reference to a form controller 
            which gets passed in to this parameter
         */
        link: function (scope, element, attrs, formCtrl) {

            //if there's any remaining errors in the server validation service then we should show them.
            var showValidation = serverValidationManager.items.length > 0;
            var hasError = false;

            //create properties on our custom scope so we can use it in our template
            scope.errorMsg = "";

            //listen for form error changes
            scope.$on("valStatusChanged", function(evt, args) {
                if (args.form.$invalid) {

                    //first we need to check if the valPropertyMsg validity is invalid
                    if (formCtrl.$error.valPropertyMsg && formCtrl.$error.valPropertyMsg.length > 0) {
                        //since we already have an error we'll just return since this means we've already set the 
                        // hasError and errorMsg properties which occurs below in the serverValidationManager.subscribe
                        return;
                    }
                    else if (element.closest(".umb-control-group").find(".ng-invalid").length > 0) {
                        //check if it's one of the properties that is invalid in the current content property
                        hasError = true;
                        //update the validation message if we don't already have one assigned.
                        if (showValidation && scope.errorMsg === "") {
                            var err;
                            //this can be null if no property was assigned
                            if (scope.property) {
                                err = serverValidationManager.getPropertyError(scope.property.alias, "");
                            }
                            scope.errorMsg = err ? err.errorMsg : "Property has errors";
                        }
                    }
                    else {
                        hasError = false;
                        scope.errorMsg = "";
                    }
                }
                else {
                    hasError = false;
                    scope.errorMsg = "";
                }
            }, true);

            //listen for the forms saving event
            scope.$on("formSubmitting", function (ev, args) {
                showValidation = true;
                if (hasError && scope.errorMsg === "") {
                    var err;
                    //this can be null if no property was assigned
                    if (scope.property) {
                        err = serverValidationManager.getPropertyError(scope.property.alias, "");
                    }
                    scope.errorMsg = err ? err.errorMsg : "Property has errors";
                }
                else if (!hasError) {
                    scope.errorMsg = "";
                }
            });

            //listen for the forms saved event
            scope.$on("formSubmitted", function (ev, args) {
                showValidation = false;
                scope.errorMsg = "";
                formCtrl.$setValidity('valPropertyMsg', true);                
            });

            //We need to subscribe to any changes to our model (based on user input)
            // This is required because when we have a server error we actually invalidate 
            // the form which means it cannot be resubmitted. 
            // So once a field is changed that has a server error assigned to it
            // we need to re-validate it for the server side validator so the user can resubmit
            // the form. Of course normal client-side validators will continue to execute.          
            scope.$watch("property.value", function(newValue) {
                //we are explicitly checking for valServer errors here, since we shouldn't auto clear
                // based on other errors. We'll also check if there's no other validation errors apart from valPropertyMsg, if valPropertyMsg
                // is the only one, then we'll clear.

                if (!newValue) {
                    return;
                }

                var errCount = 0;
                for (var e in formCtrl.$error) {
                    if (angular.isArray(formCtrl.$error[e])) {
                        errCount++;
                    }
                }

                if ((errCount === 1 && angular.isArray(formCtrl.$error.valPropertyMsg)) || (formCtrl.$invalid && angular.isArray(formCtrl.$error.valServer))) {
                    scope.errorMsg = "";
                    formCtrl.$setValidity('valPropertyMsg', true);
                }
            }, true);
            
            //listen for server validation changes
            // NOTE: we pass in "" in order to listen for all validation changes to the content property, not for
            // validation changes to fields in the property this is because some server side validators may not
            // return the field name for which the error belongs too, just the property for which it belongs.
            // It's important to note that we need to subscribe to server validation changes here because we always must
            // indicate that a content property is invalid at the property level since developers may not actually implement
            // the correct field validation in their property editors.
            
            if (scope.property) { //this can be null if no property was assigned
                serverValidationManager.subscribe(scope.property.alias, "", function(isValid, propertyErrors, allErrors) {
                    hasError = !isValid;
                    if (hasError) {
                        //set the error message to the server message
                        scope.errorMsg = propertyErrors[0].errorMsg;
                        //flag that the current validator is invalid
                        formCtrl.$setValidity('valPropertyMsg', false);
                    }
                    else {
                        scope.errorMsg = "";
                        //flag that the current validator is valid
                        formCtrl.$setValidity('valPropertyMsg', true);
                    }
                });

                //when the element is disposed we need to unsubscribe!
                // NOTE: this is very important otherwise when this controller re-binds the previous subscriptsion will remain
                // but they are a different callback instance than the above.
                element.bind('$destroy', function() {
                    serverValidationManager.unsubscribe(scope.property.alias, "");
                });
            }
        }
    };
}
angular.module('umbraco.directives').directive("valPropertyMsg", valPropertyMsg);
/**
    * @ngdoc directive
    * @name umbraco.directives.directive:valRegex
    * @restrict A
    * @description A custom directive to allow for matching a value against a regex string.
    *               NOTE: there's already an ng-pattern but this requires that a regex expression is set, not a regex string
    **/
function valRegex() {
    
    return {
        require: 'ngModel',
        restrict: "A",
        link: function (scope, elm, attrs, ctrl) {

            var flags = "";
            if (attrs.valRegexFlags) {
                try {
                    flags = scope.$eval(attrs.valRegexFlags);
                    if (!flags) {
                        flags = attrs.valRegexFlags;
                    }
                }
                catch (e) {
                    flags = attrs.valRegexFlags;
                }
            }
            var regex;
            try {
                var resolved = scope.$eval(attrs.valRegex);                
                if (resolved) {
                    regex = new RegExp(resolved, flags);
                }
                else {
                    regex = new RegExp(attrs.valRegex, flags);
                }
            }
            catch(e) {
                regex = new RegExp(attrs.valRegex, flags);
            }

            var patternValidator = function (viewValue) {
                //NOTE: we don't validate on empty values, use required validator for that
                if (!viewValue || regex.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('valRegex', true);
                    //assign a message to the validator
                    ctrl.errorMsg = "";
                    return viewValue;
                }
                else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('valRegex', false);
                    //assign a message to the validator
                    ctrl.errorMsg = "Value is invalid, it does not match the correct pattern";
                    return undefined;
                }
            };

            ctrl.$formatters.push(patternValidator);
            ctrl.$parsers.push(patternValidator);
        }
    };
}
angular.module('umbraco.directives').directive("valRegex", valRegex);
/**
    * @ngdoc directive
    * @name umbraco.directives.directive:valServer
    * @restrict A
    * @description This directive is used to associate a content property with a server-side validation response
    *               so that the validators in angular are updated based on server-side feedback.
    **/
function valServer(serverValidationManager) {
    return {
        require: 'ngModel',
        restrict: "A",
        link: function (scope, element, attr, ctrl) {
            
            if (!scope.model || !scope.model.alias){
                throw "valServer can only be used in the scope of a content property object";
            }
            var currentProperty = scope.model;

            //default to 'value' if nothing is set
            var fieldName = "value";
            if (attr.valServer) {
                fieldName = scope.$eval(attr.valServer);
                if (!fieldName) {
                    //eval returned nothing so just use the string
                    fieldName = attr.valServer;
                }
            }            

            //Need to watch the value model for it to change, previously we had  subscribed to 
            //ctrl.$viewChangeListeners but this is not good enough if you have an editor that
            // doesn't specifically have a 2 way ng binding. This is required because when we
            // have a server error we actually invalidate the form which means it cannot be 
            // resubmitted. So once a field is changed that has a server error assigned to it
            // we need to re-validate it for the server side validator so the user can resubmit
            // the form. Of course normal client-side validators will continue to execute.
            scope.$watch(function() {
                return ctrl.$modelValue;
            }, function (newValue) {
                if (ctrl.$invalid) {
                    ctrl.$setValidity('valServer', true);
                }
            });            
            
            //subscribe to the server validation changes
            serverValidationManager.subscribe(currentProperty.alias, fieldName, function (isValid, propertyErrors, allErrors) {
                if (!isValid) {
                    ctrl.$setValidity('valServer', false);
                    //assign an error msg property to the current validator
                    ctrl.errorMsg = propertyErrors[0].errorMsg;
                }
                else {
                    ctrl.$setValidity('valServer', true);
                    //reset the error message
                    ctrl.errorMsg = "";
                }
            });
            
            //when the element is disposed we need to unsubscribe!
            // NOTE: this is very important otherwise when this controller re-binds the previous subscriptsion will remain
            // but they are a different callback instance than the above.
            element.bind('$destroy', function () {
                serverValidationManager.unsubscribe(currentProperty.alias, fieldName);
            });
        }
    };
}
angular.module('umbraco.directives').directive("valServer", valServer);
/**
    * @ngdoc directive
    * @name umbraco.directives.directive:valServerField
    * @restrict A
    * @description This directive is used to associate a content field (not user defined) with a server-side validation response
    *               so that the validators in angular are updated based on server-side feedback.
    **/
function valServerField(serverValidationManager) {
    return {
        require: 'ngModel',
        restrict: "A",
        link: function (scope, element, attr, ctrl) {
            
            if (!attr.valServerField) {
                throw "valServerField must have a field name for referencing server errors";
            }

            var fieldName = attr.valServerField;
            
            //subscribe to the changed event of the view model. This is required because when we
            // have a server error we actually invalidate the form which means it cannot be 
            // resubmitted. So once a field is changed that has a server error assigned to it
            // we need to re-validate it for the server side validator so the user can resubmit
            // the form. Of course normal client-side validators will continue to execute.
            ctrl.$viewChangeListeners.push(function () {
                if (ctrl.$invalid) {
                    ctrl.$setValidity('valServerField', true);
                }
            });
            
            //subscribe to the server validation changes
            serverValidationManager.subscribe(null, fieldName, function (isValid, fieldErrors, allErrors) {
                if (!isValid) {
                    ctrl.$setValidity('valServerField', false);
                    //assign an error msg property to the current validator
                    ctrl.errorMsg = fieldErrors[0].errorMsg;
                }
                else {
                    ctrl.$setValidity('valServerField', true);
                    //reset the error message
                    ctrl.errorMsg = "";
                }
            });
            
            //when the element is disposed we need to unsubscribe!
            // NOTE: this is very important otherwise when this controller re-binds the previous subscriptsion will remain
            // but they are a different callback instance than the above.
            element.bind('$destroy', function () {
                serverValidationManager.unsubscribe(null, fieldName);
            });
        }
    };
}
angular.module('umbraco.directives').directive("valServerField", valServerField);

/**
* @ngdoc directive
* @name umbraco.directives.directive:valTab
* @restrict A
* @description Used to show validation warnings for a tab to indicate that the tab content has validations errors in its data.
* In order for this directive to work, the valFormManager directive must be placed on the containing form.
**/
function valTab() {
    return {
        require: "^form",
        restrict: "A",
        link: function (scope, element, attr, formCtrl) {
            
            var tabId = "tab" + scope.tab.id;
                        
            scope.tabHasError = false;

            //listen for form validation changes
            scope.$on("valStatusChanged", function(evt, args) {
                if (!args.form.$valid) {
                    var tabContent = element.closest(".umb-panel").find("#" + tabId);
                    //check if the validation messages are contained inside of this tabs 
                    if (tabContent.find(".ng-invalid").length > 0) {
                        scope.tabHasError = true;
                    } else {
                        scope.tabHasError = false;
                    }
                }
                else {
                    scope.tabHasError = false;
                }
            });

        }
    };
}
angular.module('umbraco.directives').directive("valTab", valTab);
function valToggleMsg(serverValidationManager) {
    return {
        require: "^form",
        restrict: "A",

        /**
            Our directive requries a reference to a form controller which gets passed in to this parameter
         */
        link: function (scope, element, attr, formCtrl) {

            if (!attr.valToggleMsg){
                throw "valToggleMsg requires that a reference to a validator is specified";
            }
            if (!attr.valMsgFor){
                throw "valToggleMsg requires that the attribute valMsgFor exists on the element";
            }
            if (!formCtrl[attr.valMsgFor]) {
                throw "valToggleMsg cannot find field " + attr.valMsgFor + " on form " + formCtrl.$name;
            }

            //if there's any remaining errors in the server validation service then we should show them.
            var showValidation = serverValidationManager.items.length > 0;
            var hasCustomMsg = element.contents().length > 0;

            //add a watch to the validator for the value (i.e. myForm.value.$error.required )
            scope.$watch(function () {
                //sometimes if a dialog closes in the middle of digest we can get null references here
                
                return (formCtrl && formCtrl[attr.valMsgFor]) ? formCtrl[attr.valMsgFor].$error[attr.valToggleMsg] : null;
            }, function () {
                //sometimes if a dialog closes in the middle of digest we can get null references here
                if ((formCtrl && formCtrl[attr.valMsgFor])) {
                    if (formCtrl[attr.valMsgFor].$error[attr.valToggleMsg] && showValidation) {                        
                        element.show();
                        //display the error message if this element has no contents
                        if (!hasCustomMsg) {
                            element.html(formCtrl[attr.valMsgFor].errorMsg);
                        }
                    }
                    else {
                        element.hide();
                    }
                }
            });
            
            //listen for the saving event (the result is a callback method which is called to unsubscribe)
            var unsubscribeSaving = scope.$on("formSubmitting", function (ev, args) {
                showValidation = true;
                if (formCtrl[attr.valMsgFor].$error[attr.valToggleMsg]) {
                    element.show();
                    //display the error message if this element has no contents
                    if (!hasCustomMsg) {
                        element.html(formCtrl[attr.valMsgFor].errorMsg);
                    }
                }
                else {
                    element.hide();
                }
            });
            
            //listen for the saved event (the result is a callback method which is called to unsubscribe)
            var unsubscribeSaved = scope.$on("formSubmitted", function (ev, args) {
                showValidation = false;
                element.hide();
            });

            //when the element is disposed we need to unsubscribe!
            // NOTE: this is very important otherwise if this directive is part of a modal, the listener still exists because the dom 
            // element might still be there even after the modal has been hidden.
            element.bind('$destroy', function () {
                unsubscribeSaving();
                unsubscribeSaved();
            });

        }
    };
}

/**
* @ngdoc directive
* @name umbraco.directives.directive:valToggleMsg
* @restrict A
* @element input
* @requires formController
* @description This directive will show/hide an error based on: is the value + the given validator invalid? AND, has the form been submitted ?
**/
angular.module('umbraco.directives').directive("valToggleMsg", valToggleMsg);

})();