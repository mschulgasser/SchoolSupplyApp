$(function () {
    $("#add-supply").on("click", function () {
        var listId = $(this).data('list-id');
        var html = `<tr><td><form id="form" method="post" action="/admin/addlistsupply"><select name="supplyId" class ="form-control">` +
                  `<option value="0">*** SELECT A SUPPLY ***</option>`;
        $.get("/admin/getsupplies", function (supplies) {
            supplies.forEach(function (s) {
                html += `<option value="` + s.id + `">` + s.name + `</option>`;
            });
            html += `</select></form></td><td><select form="form" name="quantity" class="form-control">`;
            for (var i = 1; i <= 100; i++) {
                html += `<option>` + i + `</option>`;
            }
            html += `</select><input form="form" type="hidden" value="` + listId + `" name="listId"/></td><td>` +
                `<button form="form" class="btn btn-info update">Update</button></td></tr>`;
            $("table").append(html);
        });
    });
    $(".remove").on("click", function () {
        var listId = $(this).data('list-id');
        var supplyId = $(this).data('supply-id');
        $.post("/admin/removelistsupply", { listId: listId, supplyId: supplyId }, function () {
            window.location.reload();
        });
    });
    $(".edit").on("click", function () {
        var listId = $(this).data('list-id');
        var supplyId = $(this).data('supply-id');
        var supplyName = $(this).data('supply-name');
        var quantity = $(this).data('quantity');
        $(this).closest("tr").remove();
        var html = `<tr><td>` + supplyName + `</td><td><form id="form" method="post" action="/admin/updatelistsupply">` +
            `<input type="hidden" name="supplyId" value="` + supplyId + `"/><input type="hidden" value="` + listId + `" name="listId"/>` +
            `<select name="quantity" class="form-control">`;
        for (var i = 1; i <= 100; i++) {
            if (i === quantity) {
                html += `<option selected>` + i + `</option>`;
            }
            else {
                html += `<option>` + i + `</option>`;
            }
        }
        html += `</select></form></td><td>` +
            `<button form="form" class="btn btn-info update">Update</button></td></tr>`;
        $("table").append(html);
    });
});

