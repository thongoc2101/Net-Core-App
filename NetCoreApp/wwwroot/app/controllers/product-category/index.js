﻿var productCategoryController = function() {
    this.initialize = function() {
        loadData();
    }

    function loadData() {
        $.ajax({
            type: 'GET',
            url: '/admin/productCategory/GetAll',
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
                treeArr.sort(function(a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function(target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children,
                                function(i, item) {
                                    children.push({
                                        key: item.id,
                                        value: i
                                });
                            });
                            // Update to database
                            $.ajax({
                                url: '/Admin/ProductCategory/UpdateParentId',
                                type:'POST',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function(res) {
                                    loadData();
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/ProductCategory/ReOrder',
                                type:'POST',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function(res) {
                                    loadData();
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