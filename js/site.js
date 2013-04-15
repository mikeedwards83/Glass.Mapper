(function ($) {
    $(function () {

        $("pre.github").each(function () {

            var $pre = $(this);
            $pre.addClass("prettyprint");

            var sha = $pre.attr("sha");
            var path = $pre.attr("file");
            var ref = $pre.attr("ref");

            if (sha !== undefined || path !== undefined) {

                var commit = new GitHubCommit({
                    username: "mikeedwards83",
                    reponame: "Glass.Mapper",
                    fileSHA: sha,
                    filePath: path,
                    ref: ref
                });

                commit.fetch(function(content) {
                    $pre.html(content);
                    prettyPrint();
                });
            }
        });//pre.github.each

        prettyPrint();
    });
})(jQuery);

//utilities
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}