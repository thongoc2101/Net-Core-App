var productController = function() {
    this.initialize = function() {
        loadCategories();
        loadData();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        // Init validation
        $('#frmMaintenance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: {required: true},
                ddlCategoryIdM: {required: true},
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });

        $('#ddlShowPage').on('change', function () {
            app.configs.pageSize = $(this).val();
            app.configs.pageIndex = 1;
            loadData(true);
        });
        $('#btnSearch').on('click',
            function() {
                loadData();
            });
        $('#txtKeyword').on('keypress',
            function(e) {
                if (e.which === 13) {
                    loadData();
                }
            });
        $('#btnCreate').on('click',
            function() {
                resetFormMaintenance();
                initTreeDropDownCategory();
                $('#modal-add-edit').modal('show');
            });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintenance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var categoryId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();
                var unit = $('#txtUnitM').val();
                var price = $('#txtPriceM').val();
                var originalPrice = $('#txtOriginalPriceM').val();
                var promotionPrice = $('#txtPromotionPriceM').val();
                //var image = $('#txtImageM').val();
                var tags = $('#txtTagM').val();
                var seoKeyword = $('#txtMetakeywordM').val();
                var seoMetaDescription = $('#txtMetaDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var content = CKEDITOR.instances.txtContent.getData();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var hot = $('#ckHotM').prop('checked');
                var showHome = $('#ckShowHomeM').prop('checked');
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        CategoryId: categoryId,
                        Image: '',
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "json",
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        app.notify('Update product successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintenance();
                        app.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        app.notify('Has an error in save product progress', 'error');
                        app.stopLoading();
                    }
                });
                return false;
            }
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: 'GET',
                data: {
                    id: that
                },
                dataType: 'json',
                url: '/Admin/Product/GetProductById',
                beforeSend: function () {
                    app.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);
                    $('#txtDescM').val(data.Description);
                    $('#txtUnitM').val(data.Unit);
                    $('#txtPriceM').val(data.Price);
                    $('#txtOriginalPriceM').val(data.OriginalPrice);
                    $('#txtPromotionPriceM').val(data.PromotionPrice);
                    // $('#txtImageM').val(data.ThumbnailImage);
                    $('#txtTagM').val(data.Tags);
                    $('#txtMetakeywordM').val(data.SeoKeywords);
                    $('#txtMetaDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);
                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckHotM').prop('checked', data.HotFlag);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#modal-add-edit').modal('show');
                    app.stopLoading();
                },
                error: function (error) {
                    app.notify('Has an Error', 'error');
                    app.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            app.confirm('Are you sure', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/Delete',
                    data: {
                        id: that
                    },
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        app.notify('Delete success', 'success');
                        app.stopLoading();
                        //location.reload();
                        loadData(true);
                    },
                    error: function () {
                        app.notify('Has an error', 'error');
                        app.stopLoading();
                    }
                });
            });
        });

        // event upload image
        $('#btnSelectImg').on('click',
            function() {
                $('#fileInputImage').click();
            });

        // event onchange fileInputImage
        $('#fileInputImage').on('change',
            function() {
                var fileUpload = $(this).get(0);
                var files = fileUpload.files;
                var data = new FormData();
                for (var i = 0; i < files.length; i++) {
                    data.append(files[i].name, files[i]);
                }
                $.ajax({
                    type: "POST",
                    url: "/Admin/Upload/UploadImage",
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (path) {
                        $('#txtImageM').val(path);
                        app.notify('Upload image success', 'success');
                    },
                    error: function (status) {
                        app.notify('Has an error in uploading image progress', 'error');
                    }
                });
            });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Product/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();

                }
            });
            return false;
        });

        $('#btnExport').on('click',
            function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/ExportExcel',
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        // return file, redirect to file => download
                        window.location.href = response;
                        app.stopLoading();
                    },
                    error: function () {
                        app.notify('Has an error', 'error');
                        app.stopLoading();
                    }
                });
            });
    
    }

    function registerControls() {
        CKEDITOR.replace('txtContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal',
                    $.proxy(function (e) {
                            if (
                                this.$element[0] !== e.target &&
                                    !this.$element.has(e.target).length
                                    // CKEditor compatibility fix start.
                                    &&
                                    !$(e.target).closest('.cke_dialog, .cke').length
                                // CKEditor compatibility fix end.
                            ) {
                                this.$element.trigger('focus');
                            }
                        },
                        this));
        };

    }

    function loadCategories() {
        $.ajax({
            type: 'GET',
            url: "/admin/product/GetAllCategories",
            dataType: 'json',
            success: function(response) {
                var render = "<option value=''>--Select Category--</option>";
                $.each(response,
                    function(i, item) {
                        render += "<option value='" + item.Id + "'>"+item.Name+"</option>";
                    });
                $('#ddlCategorySearch').html(render);
            },
            error: function(status) {
                console.log(status);
                app.notify('Cannot loading product category data ', 'error');
            }
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: "/admin/product/GetAllPaging",
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: app.configs.pageIndex,
                pageSize: app.configs.pageSize
            },
            dataType: 'json',
            success: function(response) {
                if (response.RowCount === 0) {
                    $('#table-content').html(render);
                    $('#lblTotalRecords').text(response.RowCount);
                    $('#paginationUL').css('display','none');
                    app.notify('No data', 'error');
                    
                } else {
                    $.each(response.Results,
                        function(i, item) {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Price: item.Price,
                                    Category: item.ProductCategory.Name,
                                    Image: item.image= '<img src="/vendor/gentelella/production/user.png" width = "25px"></img>',
                                    CreatedDate: app.dateFormatJson(item.DateCreated),
                                    Status: app.getStatus(item.Status)
                                });
                            if (render !== '') {
                                $('#lblTotalRecords').text(response.RowCount);
                                $('#table-content').html(render);
                            }
                            wrapPaging(response.RowCount, function () {
                                loadData();
                            }, isPageChanged);
                        });
                }
            },
            error: function(status) {
                console.log(status);
                app.notify('Cannot loading data ', 'error');
            }
        });
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / app.configs.pageSize);
        //Unbind pagination if it existed or click change pageSize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'First',
            prev: 'Previous',
            next: 'Next',
            last: 'Last',
            onPageClick: function (event, p) {
                //Assign if p!==CurrentPage
                if (app.configs.pageIndex !== p) {
                    app.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }

    function resetFormMaintenance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');
        $('#txtDescM').val('');
        $('#txtUnitM').val('');
        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('0');
        $('#txtPromotionPriceM').val('0');
        //$('#txtImageM').val('');
        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');
        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = app.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
}