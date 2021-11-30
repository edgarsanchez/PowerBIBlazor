export function embedReport(containerId, reportId, embedUrl, token) {
    // 1 - get DOM object for div that is report container 
    var reportContainer = document.getElementById(containerId)

    // 2 - embed report using the Power BI JavaScript API.
    var models = window['powerbi-client'].models

    var config = {
        type: 'report',
        id: reportId,
        embedUrl: embedUrl,
        accessToken: token,
        permissions: models.Permissions.All,
        tokenType: models.TokenType.Embed,
        viewMode: models.ViewMode.View,
        settings: {
            panes: {
                filters: { expanded: false, visible: true },
                pageNavigation: { visible: false }
            }
        }
    }

    // Embed the report and display it within the div container.
    var report = powerbi.embed(reportContainer, config)

    // 3 - add logic to resize embed container on window resize event
    var heightBuffer = 32;
    var newHeight = $(window).height() - ($("header").height() + heightBuffer)
    $("#" + containerId).height(newHeight)
    $(window).resize(() => {
        var newHeight = $(window).height() - ($("header").height() + heightBuffer)
        $("#" + containerId).height(newHeight)
    })
}
