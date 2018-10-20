var productCategoryController = function() {
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
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true
                });
            },
            error: function (error) {
                app.notify(error, 'error');
            }
        });
    };
}