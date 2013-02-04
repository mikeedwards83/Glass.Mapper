(function($) {
    $(function () {
        $("pre.github").each(function () {

            var $pre = $(this);
            var sha = $pre.attr("sha");
            var commit = new GitHubCommit({
                username: "mikeedwards83",
                reponame: "Glass.Mapper",
                fileSHA: sha
            });

            commit.fetch(function(content) {
                $pre.html(content);
                prettyPrint();
            });
        });

    });
})(jQuery);