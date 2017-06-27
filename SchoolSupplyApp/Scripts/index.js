$(function () {
    var index = 1;
    $("#add-child").on("click", function () {
        var html = `<tr><td><input type="text" class="form-control" placeholder="Child's Name" name="children[` + index + `].name" />` +
                    `</td><td><select class="form-control school-select" name="children[` + index + `].schoolId" data-index="` + index + `">` +
                    `<option value="0">*** SELECT A SCHOOL ***</option>`;
        $.get("/home/getschools", function(schools){
            schools.forEach(function(s){
                html += ` <option value="` + s.id + `">` + s.name +`</option>`;
            });
            html += `</select></td><td> <select  class="form-control grade-select" name="children[` + index + `].grade" data-index ="` + index +
                                `"><option value="0">*** SELECT A GRADE ***</option></select></tr>`;
            $("table").append(html);
            var element = $("table").last('row').last('td');
            index++;
        });                      
    });
    $(".table").on('change', ".school-select", function () {
        console.log('school select changed');
        var id = $(this).val();
        var gradeSelect = $(this).closest('tr').find('td:eq(2)').find('select');
        gradeSelect.children('option:gt(0)').remove();
        $.get("/home/getgradesforschool", { schoolId: id }, function (data) {
            console.log(data.grades);
            var html;
            data.grades.forEach(function (g) {
                html += `<option>` + g + `</option>`;
            });
            gradeSelect.append(html);
        });

    });

    function refillGradeSelect(schoolId, element) {
    }
});