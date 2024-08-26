
$(document).ready(function () {
    // Initial call to validateRange
    validateRange();
    showCategory(3);
});
var urls = {
    urlQuickView: SiteConfig.gSiteAdrs + 'Customer/Home/_QuickView',
    urlCategory: SiteConfig.gSiteAdrs + 'Customer/Category/_Index'
}

let minValue = document.getElementById("min-value");
let maxValue = document.getElementById("max-value");

const rangeFill = document.querySelector(".price-container .range-fill");


const inputElements = document.querySelectorAll(".price-container input");
const licategoryElements = document.querySelectorAll(".nav-category .nav-item a");

// Add an event listener to each input element
inputElements.forEach((element) => {
    element.addEventListener("input", validateRange);
});

// Add an event listener to each li category element

licategoryElements.forEach((element) => {
    element.addEventListener("click", function () {
        let id = parseInt(this.dataset.cateid)
        showCategory(id);
    });
});

// Function to validate range and update the fill color on slider
function validateRange() {
    let minPrice = parseInt(inputElements[0].value);
    let maxPrice = parseInt(inputElements[1].value);

    if (minPrice > maxPrice) {
        let tempValue = maxPrice;
        maxPrice = minPrice;
        minPrice = tempValue;
    }

    const minPercentage = ((minPrice - 10) / 490) * 100;
    const maxPercentage = ((maxPrice - 10) / 490) * 100;

    rangeFill.style.left = minPercentage + "%";
    rangeFill.style.width = maxPercentage - minPercentage + "%";

    minValue.innerHTML = "$" + minPrice;
    maxValue.innerHTML = "$" + maxPrice;
}
function openQuickViewModal(productID) {
    $.ajax({
        url: urls.urlQuickView,
        type: 'GET',
        data: {
            productID: productID
        },
        success: function (data) {
            $("#product-QuickView").html(data);
            $("#product-QuickView").dialog({
                width: 800,
                height: 600,
                modal: true,
                lgClass: true,
                show: {
                    effect: 'fade',
                    duration: 200 //at your convenience
                },
                hide: {
                    effect: 'fade',
                    duration: 200 //at your convenience
                }
            });
            $('#productModal').slick({
                infinite: true
            });
        }
    })
}
function showCategory(id) {
    $.ajax({
        url: urls.urlCategory,
        type: "GET",
        data: {
            Id: id,
        },
        success: function (data) {
            $("#category-card").html(data);
        }
    });
}

