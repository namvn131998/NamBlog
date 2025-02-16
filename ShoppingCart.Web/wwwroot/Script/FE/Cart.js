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
// custom button quantity
let number = 1;

document.getElementById("button-plus").addEventListener("click", () => {
    number++;
    document.getElementById("quantityProduct").value = number;
});

document.getElementById("button-minus").addEventListener("click", () => {
    number--;
    if (number >= 0)
        document.getElementById("quantityProduct").value = number;
    else {
        number = 0;
        document.getElementById("quantityProduct").value = number;
    }
});
//end