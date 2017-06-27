$(function () {
    var index = 1;
    $("#add-supply").on("click", function () {
        var html = `<tr><td><select name="listSupplies[` + index + `].SupplyId" class ="form-control">` + 
                    `<option value="0">*** SELECT A SUPPLY ***</option>`;
        $.get("/admin/getsupplies", function (supplies) {
            supplies.forEach(function (s) {
                console.log(s);
                html += `<option value="` + s.id + `">` + s.name + `</option>`;
            });
            html += `</select></td><td><select name="listSupplies[` + index + `].Quantity" class="form-control supply-select">`;
            for (var i = 1; i <= 100; i++) {
                html += `<option>` + i + `</option>`;
            }
            html += `</select></td></tr>`;
            $("table").append(html);
            index++;
        });
        $("#add-list").attr('disabled', !isValid());
    });
    $(".form-control").on('change', function () {
        $("#add-list").attr('disabled', !isValid());
    });
    $(".form-control").on('keyup', function () {
        $("#add-list").attr('disabled', !isValid());
    });
    function isValid() {
            var empty = $(".supply-select").filter(function () {
                return this.value === 0;
            });
         var isValid = /*$("#grade").text() != "" &&*/ empty.length === 0;
       //// var isValid = $("#grade").text() == "" && $(".supply-select").selected != 0;
       // var isValid = $("input:empty").length !== 0;
        console.log(isValid);
        return isValid;
    }
});