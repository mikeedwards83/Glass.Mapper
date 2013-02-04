(function($) {
    $(function () {
        $("pre.github").forEach(function () {

            var $pre = $(this);
            var sha = $pre.Attr("sha");
            var commit = new GitHubCommit({
                username: "mikeedwards83",
                reponame: "Glass.Mapper",
                fileSHA: sha
            });

            commit.fetch(function(content) {
                $pre.html(content);
            });
        });

    });
})(jQuery);