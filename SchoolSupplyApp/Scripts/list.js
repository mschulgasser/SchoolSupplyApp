$(function () {
    $("#add-supply").on("click", function () {
        var listId = $(this).data('list-id');
        var html = `<tr><td><select id="supplyId" class ="form-control">` +
                  `<option value="0">*** SELECT A SUPPLY ***</option>`;
        $.get("/admin/getsupplies", function (supplies) {
            supplies.forEach(function (s) {
                html += `<option value="` + s.id + `">` + s.name + `</option>`;
            });
            html += `</select></td><td><select id="quantity" class="form-control">`;
            for (var i = 1; i <= 100; i++) {
                html += `<option>` + i + `</option>`;
            }
            html += `</select></td><td>` +
               `<button class="btn btn-info add" data-list-id="` + listId + `">Add</button></td></tr>`;
            $("table").append(html);
        });
    });
    $("table").on("click", ".add", function () {
        var button = $(this);
        var params = {
            listId: button.data("list-id"),
            quantity: $("#quantity").val(),
            supplyId: $("#supplyId").val()
        }
        $.post("/admin/addlistsupply", params, function (listSupplies) {
            refillTable(listSupplies);
        });
    });
    $("table").on("click", ".remove", function () {
        var listId = $(this).data('list-id');
        var supplyId = $(this).data('supply-id');
        $.post("/admin/removelistsupply", { listId: listId, supplyId: supplyId }, function (listSupplies) {
            refillTable(listSupplies);
        });
    });
    $("table").on("click", ".edit", function () {
        var listId = $(this).data('list-id');
        var supplyId = $(this).data('supply-id');
        var supplyName = $(this).data('supply-name');
        var quantity = $(this).data('quantity');
        $(this).closest("tr").remove();
        var html = `<tr><td>` + supplyName + `</td><td>` +
            `<input type="hidden" id="supplyId" value="` + supplyId + `"/><input type="hidden" value="` + listId + `" id="listId"/>` +
            `<select id="quantity" class="form-control">`;
        for (var i = 1; i <= 100; i++) {
            if (i === quantity) {
                html += `<option selected>` + i + `</option>`;
            }
            else {
                html += `<option>` + i + `</option>`;
            }
        }
        html += `</select></td><td>` +
            `<button class="btn btn-info update">Update</button></td></tr>`;
        $("table").append(html);
    });
    $("table").on("click", ".update", function () {
        var button = $(this);
        var params = {
            listId: $("#listId").val(),
            quantity: $("#quantity").val(),
            supplyId: $("#supplyId").val()
        }
        $.post("/admin/updatelistsupply", params, function (listSupplies) {
            refillTable(listSupplies);
        });
    });
    function refillTable(listSupplies) {
        $("table tr:gt(0)").remove();
        listSupplies.forEach(function (ls) {
            $("table").append(`<tr><td>` + ls.supplyName + `</td><td>`
                + ls.quantity + `</td><td><button class="btn btn-primary edit" data-quantity="`
                + ls.quantity + `" data-supply-id="` + ls.supplyId + `"data-supply-name="` +
                ls.supplyName + `" data-list-id="` + ls.listId + `">Edit</button>` +
                `<button class="btn btn-danger remove" data-supply-id="` + ls.supplyId + `" data-list-id="` +
                ls.listId + `">Remove</button></td></tr>`);
        });
    }
});

