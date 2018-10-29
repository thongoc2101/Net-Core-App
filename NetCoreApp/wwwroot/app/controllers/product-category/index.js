var productCategoryController = function () {
    this.initialize = function () {
        getProductCategory();
        registerEvent();
    };

    function registerEvent() {
        $('#btnCreate').off('click').on('click',
            function () {
                initTreeDropDownCategory();
                $('#modal-add-edit').modal('show');
            });
        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            $.ajax({
                type: "GET",
                url: "/Admin/ProductCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    app.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);
                    $('#txtDescM').val(data.Description);
                    $('#txtImageM').val(data.ThumbnailImage);
                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);
                    $('#modal-add-edit').modal('show');
                    app.stopLoading();
                },
                error: function (status) {
                    app.notify('Có lỗi xảy ra', 'error');
                    app.stopLoading();
                }
            });
        });
        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            app.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        app.notify('Deleted success', 'success');
                        app.stopLoading();
                        getProductCategory();
                    },
                    error: function (status) {
                        app.notify('Has an error in deleting progress', 'error');
                        app.stopLoading();
                    }
                });
            });
        });
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();
                var image = $('#txtImageM').val();
                var order = parseInt($('#txtOrderM').val());
                var homeOrder = $('#txtHomeOrderM').val();
                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                var showHome = $('#ckShowHomeM').prop('checked');
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
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
                        app.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        app.stopLoading();
                        getProductCategory(true);
                    },
                    error: function () {
                        app.notify('Has an error in update progress', 'error');
                        app.stopLoading();
                    }
                });
            }
            return false;
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
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');
        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');
        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
    }
    //Get Dropdown Tree
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
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
    function getProductCategory() {
        $.ajax({
            type: 'GET',
            url: '/admin/productcategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response,
                    function (i, item) {
                        data.push({
                            id: item.Id,
                            text: item.Name,
                            parentId: item.ParentId,
                            sortOrder: item.SortOrder
                        });
                    });
                var treeArr = app.unflattern(data);

                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        //Remove right menu default
                        e.preventDefault();
                        //Get ID when right click
                        $('#hidIdM').val(node.id);
                        $('#contextMenu').menu('show',
                            {
                                left: e.pageX,
                                top: e.pageY
                            });
                    },
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point == 'append') {
                            var children = [];
                            $.each(targetNode.children,
                                function (i, item) {
                                    children.push({
                                        key: item.id,
                                        value: i
                                    });
                                });
                            //Update Db
                            $.ajax({
                                url: '/admin/ProductCategory/UpdateParentId',
                                type: 'POST',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (response) {
                                    getProductCategory();
                                }
                            });
                        } else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/admin/ProductCategory/ReOrder',
                                type: 'POST',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (response) {
                                    getProductCategory();
                                }
                            });
                        }
                    }
                });
            },
            error: function (error) {
                app.notify(error, 'error');
            }
        });
    };
}