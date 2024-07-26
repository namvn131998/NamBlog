var ProductJS = {
    urls: {
        Delete: SiteConfig.gSiteAdrs + "/Product/Delete",
        List: SiteConfig.gSiteAdrs + "/Product/_List",
        ShowUploadFile: SiteConfig.gSiteAdrs + "/UploadFile/ShowUploadFile",
        CreateOrUpdate: SiteConfig.gSiteAdrs + "/Product/CreateOrUpdate",
        SubmitAddFileProduct: SiteConfig.gSiteAdrs + "/UploadFile/AddListProductThumbnail",
        ShowListMedia: SiteConfig.gSiteAdrs + "/UploadFile/ShowListMedia",
        DeleteMedia: SiteConfig.gSiteAdrs + "/UploadFile/DeleteMedia",
    },
    Delete: function (id) {
        $.ajax({
            url: ProductJS.urls.Delete,
            type: "POST",
            data: { Id: id },
            success: function (data) {
                if (data.result == "OK")
                    alert(languageScript.Delete);
                ProductJS.GetList();
            }
        })
    },
    GetList: function () {
        $.ajax({
            type: "GET",
            url: ProductJS.urls.List,
            data: {
                SearchValue: $('#searchData').val(),
                sortBy: "Name",
                sortDirection: "DESC"
            },
            success: function (data) {
                $("#page-data").html(data);
            }
        })
    },
    ShowUploadFile: function (isProduct, productID) {
        $.ajax({
            type: "GET",
            url: ProductJS.urls.ShowUploadFile,
            data: {
                isProduct: "True",
                productID: productID
            },
            success: function (data) {
                $("#modalUploadProductImage").html(data);
                $("#modalUploadProductImage").dialog({
                    modal: true,
                    width: 500,
                    lgClass: true
                });
                $("#modalUploadProductImage").dialog("open");
            }
        })
    },
    CreateOrUpdate: function () {
        var data = $("form").serialize();
        var data = new FormData($('#CreateOrUpdate')[0]);
        $.ajax({
            type: "POST",
            url: ProductJS.urls.CreateOrUpdate,
            data: data,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.result == "OK")
                    ProductJS.ShowUploadFile("True", parseInt(data.productID));
            }
        })
    },
    SubmitAddFileProduct: function (MediaTypeID) {
        var data = new FormData($('#addMediaFileProduct')[0]);
        var productID = $("#productID").val();
        data.append("productID", productID);
        data.append("MediaTypeID", MediaTypeID);
        if ($("#addMediaFileProduct").valid()) {
            $.ajax({
                url: ProductJS.urls.SubmitAddFileProduct,
                type: 'POST',
                data: data,
                contentType: false,
                processData: false,
                success: function (data) {
                    $("#modalUploadProductImage").dialog("close");
                    if (data.value == "OK") {
                        alert("Upload image success!")
                    } else {
                        alert("Image has duplicate!")
                    }
                }
            });
        }

    },
    ShowListMedia: function (mediaid) {
        $.ajax({
            url: ProductJS.urls.ShowListMedia,
            type: "GET",
            data: {
                mediaid: mediaid
            },
            success: function (data) {
                $('#dvListMedia').append(data);
            }
        })
    },
    DeleteMedia: function (meidaid, fileName, productID) {
        var userID = $("#UserID").val();
        $.ajax({
            url: ProductJS.urls.DeleteMedia,
            type: "POST",
            data: {
                mediaid: meidaid,
                UserID: userID,
                FileName: fileName,
                productID: productID
            },
            success: function (data) {
                alert(data.result);
                window.location.reload();
            }
        })
    }
}
