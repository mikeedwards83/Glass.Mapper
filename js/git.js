/**
 *    GitHubCommit.js
 *
 *    Embed a given GitHub commit, with optional syntax highlighting
 *    support.
 *
 *    @Author: Ryan McGrath <ryan@venodesigns.net>, @mygengo_dev
 *    @Credits: Chris Veness, much of the UTF-8 handling code
 *    @Requires: Nothing extra. ;)
 */

;(function(w, d, exists) {
    if(exists !== 'undefined') return false;

    /**
     *    new GitHubCommitEmbed({});
     *    
     *    Handles embedding GitHub commits, etc. Requires a few things, passed in one object.
     *
     *    @param {fileSHA}, str, The SHA of the file to grab a blob for. A file SHA is different from
     *        a commit SHA; it can be obtained by running "git hash-object <<filename>>".
     *    @param {username}, str, The username of the owner of the repository being queried.
     *    @param {reponame}, str, The name of the repository you're trying to pull a blob from.
     *    @param {node}, DOMNode or str, DOM node reference or if string pass an ID.
     *    @returns {object} A GitHubCommit instance.
     */
    var GitHubCommit = w.GitHubCommit = function(options) {
        this.fileSHA = options.fileSHA;
        this.username = options.username;
        this.reponame = options.reponame;
        this.filePath = options.filePath;
        this.ref = options.ref;
        return this;
    };
    
    GitHubCommit.prototype = {
        /**
         *    Sensible defaults; some don't do this, I do. Shh.
         */
        fileSHA: undefined,
        username: undefined,
        reponame: undefined,
        content: undefined,
        filePath: undefined,
        ref: "master",
        
        /**
         *    GitHubCommit.fetch(optional_callbackfn);
         *
         *    The actual call that does DOM manipulation; provided separately so people
         *    can handle their own unique DOMReady scenarios.
         *
         *    @returns {object} The GitHubCommit instance this was called on.
         */
        fetch: function(optional_callbackfn) {
            if(typeof optional_callbackfn === 'function') {
                this.callbackfn = GitHubCommit.util.bind(this, optional_callbackfn);
            }
            
            /* String concatenation is ugly. */
            var ghurl = '';
            
            if(this.fileSHA === undefined){
               ghurl = ['https://api.github.com/repos/',this.username,'/',this.reponame,'/contents/',this.filePath,'?ref=', this.ref].join('');

            }
            else{
               ghurl = ['https://api.github.com/repos/',this.username,'/',this.reponame,'/git/blobs/',this.fileSHA,'?ref=', this.ref].join('');
            }
       console.log(ghurl);
            GitHubCommit.util.jsonp(ghurl, this._parseSHAData, this);
            return this;
        },
        
        /**
         *    GitHubCommit._parseSHAData();
         *
         *    Handles decoding and injecting the returned base64 encoded data.
          *    
         *    @param {object} a JSON data structure/object. 
         *    @returns {object} The GitHubCommit instance this was called on.
         */
        _parseSHAData: function(resp) {
            var content = GitHubCommit.util.decodeBase64(resp.data.content.replace(/\n/g, ''));
            
            if(typeof this.node === 'string') this.node = d.getElementById(this.node);
            this.content = content;
            if(typeof this.callbackfn === 'function') this.callbackfn(content);
        }
    };

    GitHubCommit.util = {
        DEBUG: false,

        /**
         *    GitHubCommit.util.bind(bindReference, fn)
         *
         *    Takes a reference (an object to scope to "this" at a later runtime) and binds it to a function (fn).
         *
         *    @param bindReference - An object to set as the "this" reference for a later function call.
         *    @param fn - A function to bind the "this" object for.
         *    @returns fn - A new function to pass around, wherein it's all scoped as you want it.
         */
        bind: function(bindReference, fn) {
            return function() {
                return fn.apply(bindReference, arguments);
            };
        },
        
        /**
         *    GitHubCommit.util.loadScript(src, optional_callbackfn)
         *
         *    Handles pulling down script tags, accepts an optional callback function that will
         *    fire when the script fires its ready event (Note: this is NOT a JSON-P callback, see the next function for that).
         *
         *    @param src - Required, the source URI for the script we're gonna toss onto the page.
         *    @param optional_callbackfn - Optional, a callback function that will execute once this script is done.
         *    @returns - void (nothing)
         */
        loadScript: function(src, optional_callbackfn) {
            var newScript = d.createElement("script");
            newScript.setAttribute('charset', 'utf8');
            newScript.type = "text/javascript";
            newScript.setAttribute("src", src);

            /**
             *    For newer browsers that support this, we're fine to set this - it's basically stating
             *    that we don't have any dependencies here to worry about and we're fine to let this go
             *    out on its own and report back when done.
             *
             *    If you were to have a dependency situation, you'd probably end up chaining loadScript callbacks to
             *    achieve your desired order.
             */
            newScript.setAttribute("async", "true");

            /**
             *    Automagically handle cleanup of injected script tags, so we don't litter someone's DOM
             *    with our stuff. This branches for obvious reasons - i.e, IE. 
             */
            if(newScript.readyState) {
                newScript.onreadystatechange = function() {
                    if(/loaded|complete/.test(newScript.readyState)) {
                        newScript.onreadystatechange = null;
                        if(typeof optional_callbackfn !== "undefined") optional_callbackfn();
                        !GitHubCommit.util.DEBUG && newScript && d.documentElement.firstChild.removeChild(newScript);
                    }
                }
            } else {
                newScript.addEventListener("load", function() {
                    if(typeof optional_callbackfn !== "undefined") optional_callbackfn();
                    !GitHubCommit.util.DEBUG && newScript && d.documentElement.firstChild.removeChild(newScript);
                }, false);
            }

            /**
             *    Install it in an easy to retrieve place (that's also consistent - god forbid, someone might be using frames somewhere...?). 
             */
            d.documentElement.firstChild.appendChild(newScript);
        },

        /**
         *    GitHubCommit.util.jsonp(src, callbackfn, optional_scope, optional_fn_name)
         *
         *    JSON-P function; fetches an external resource via a dynamically injected <script> tag. Said source needs to be wrapping its response
         *    JSON in a Javascript callback for this to work; what this function does is properly scope arguments and temporarily set a function in the
         *    global scope that can be accessed by any newly loaded script. 
         *    
         *    Once the script loads, we pass the results it gets to the real function, and then clean up the mess this script created as best we can.
         *
         *    Note: this function piggybacks on top of GitHubCommit.util.loadScript (up above); the logic contained in this function is moreso used for handling
         *        cleanup specific to the JSON-P call style (e.g, an odd global function sitting out there).
         *    
         *    @param src - Required, the source URI for the script we're gonna toss onto the page.
         *    @param callbackfn - Required, a callback function that will execute once this script is done (and be passed the proper results for).
         *    @param optional_scope - Optional, a scope to tie the callback structure to.
         *    @param optional_fn_name - Optional, a function name (as a String) to have this JSON-P callback pass things to (otherwise we generate a randomized name).
         *    @returns void (nothing)
         */
        jsonp: function(src, callbackfn, optional_scope, optional_fn_name) {
            var callback = typeof optional_scope !== "undefined" ? GitHubCommit.util.bind(optional_scope, callbackfn) : callbackfn,
                callbackGlobalRef = typeof optional_fn_name !== "undefined" ? optional_fn_name : "GitHubCommitJSONPCallback_" + parseInt(Math.random() * 100000),
                apiURL = src + (src.indexOf("?") > -1 ? "&callback=" : "?callback=") + callbackGlobalRef,
                globalCallback = null;

            /**
             *    After the callback has actually been fired, we should attempt cleanup. In the case of this widget it's probably
             *    not the worst thing in the world, but eh, no sense in being kind.
             */
            globalCallback = GitHubCommit.util.bind(this, function(results) {
                callback(results);
                try {
                    /**
                     *    Eh why the hizell not. 
                     */
                    delete w[callbackGlobalRef];
                } catch(e) {
                    /**
                     *    Break references for great justice. 
                     */
                    w[callbackGlobalRef] = null;
                }
            });
        
            /**
             *  We need a global reference to a bound function to execute once the data endpoint downloads
             *	and fires. (Generally namespace'd [to a degree] with a hint of random-ness).
             */
            w[callbackGlobalRef] = globalCallback;

            /**
             *    Now that we've got that all in place, we can defer over to loadScript. Note that in a way
             *    we're ignoring callbacks here - our true callback (that gets the JSON) is handled in the actual
             *    response.
             *
             *  @note: Bind this to the jsonp scope, so the callback chain persists and we can clean up the global
             *        scope when we're done. Script tag cleanup is handled naturally by GitHubCommit.util.loadScript().
             */
            GitHubCommit.util.loadScript(apiURL);
        },
        
        /**
         *    The following code was mostly written by others; credit notices/etc have been left in where
         *    they were provided, but if any of the code below belongs to you please feel free to fork, fix,
         *    and pull request (with some kind of background info) on GitHub.
         *
         *    - Ryan (ryan@venodesigns.net), who doesn't enjoy writing code that's been done before. ^_^
         */        
        
        /**
         * Decode string from Base64, as defined by RFC 4648 [http://tools.ietf.org/html/rfc4648]
         * (instance method extending String object). As per RFC 4648, newlines are not catered for.
         *
         * @param {String} str The string to be decoded from base-64
         * @param {Boolean} [utf8decode=false] Flag to indicate whether str is Unicode string to be decoded 
         *   from UTF8 after conversion from base64
         * @returns {String} decoded string
         */ 
        decodeBase64: function(str, utf8decode) {
            /* Original version didn't default to native; fix this. */
            if(typeof w.atob === 'function') return decodeURIComponent(escape(atob(str))).replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/\t/g, '&nbsp;&nbsp;&nbsp;&nbsp;');

            var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            var o1, o2, o3, h1, h2, h3, h4, bits, i = 0,
                ac = 0,
                dec = "",
                tmp_arr = [];

            if (!data) return data;
            data += '';

            do { // unpack four hexets into three octets using index points in b64
                h1 = b64.indexOf(data.charAt(i++));
                h2 = b64.indexOf(data.charAt(i++));
                h3 = b64.indexOf(data.charAt(i++));
                h4 = b64.indexOf(data.charAt(i++));

                bits = h1 << 18 | h2 << 12 | h3 << 6 | h4;

                o1 = bits >> 16 & 0xff;
                o2 = bits >> 8 & 0xff;
                o3 = bits & 0xff;

                if (h3 == 64) tmp_arr[ac++] = String.fromCharCode(o1);
                else if (h4 == 64) tmp_arr[ac++] = String.fromCharCode(o1, o2);
                else tmp_arr[ac++] = String.fromCharCode(o1, o2, o3);
            } while (i < data.length);

            dec = tmp_arr.join('');
            dec = GitHubCommit.util.utf8decode(dec);

            return dec.replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/\\t/g, '    ').replace(/\t/g, '&nbsp;&nbsp;&nbsp;&nbsp;');;
        },
        
        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
        /*  Utf8 decode function: decode between multi-byte Unicode characters and UTF-8 multiple          */
        /*              single-byte character encoding (c) Chris Veness 2002-2010                         */
        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */
        /**
         *    Decode utf-8 encoded string back into multi-byte Unicode characters
         *
         *    @param {String} strUtf UTF-8 string to be decoded back to Unicode
         *    @returns {String} decoded string
         */
        utf8decode: function(strUtf) {
            // note: decode 3-byte chars first as decoded 2-byte strings could appear to be 3-byte char!
            var strUni = strUtf.replace(/[\u00e0-\u00ef][\u0080-\u00bf][\u0080-\u00bf]/g,  /* 3-byte chars */ function(c) {  // (note parentheses for precence)
                var cc = ((c.charCodeAt(0)&0x0f)<<12) | ((c.charCodeAt(1)&0x3f)<<6) | ( c.charCodeAt(2)&0x3f); 
                return String.fromCharCode(cc); 
            });
            return strUni.replace(/[\u00c0-\u00df][\u0080-\u00bf]/g, /* 2-byte chars */    function(c) {  // (note parentheses for precence)
                var cc = (c.charCodeAt(0)&0x1f)<<6 | c.charCodeAt(1)&0x3f;
                return String.fromCharCode(cc); 
            });
        }
    };
})(window, document, typeof window.GitHubCommit);