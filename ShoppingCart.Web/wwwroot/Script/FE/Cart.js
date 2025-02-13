$(document).ready(function () {

});
var urls = {
    urlRemoveCart: SiteConfig.gSiteAdrs + 'Customer/Cart/RemoveCartItem',
}
function removeCartItem(id) {
    $.ajax({
        url: urls.urlRemoveCart,
        type: 'POST',
        data: {
            Id: id
        },
        success: function (data) {
            if (data.message == "OK") {
                window.location.reload();
            }
        }
    });
}