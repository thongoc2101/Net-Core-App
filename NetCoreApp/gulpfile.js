/// <binding Clean='clean' />
"use strict";
var gulp = require("gulp");
var runSequence = require("run-sequence");
// Dependency Dirs
//Node Module
var nodeLibs = {
    "jquery": {
        "dist/*": ""
    },
    "bootstrap": {
        "dist/**/*": ""
    },
    "font-awesome": {
        "css/**/*": "css",
        "fonts/**/*": "fonts"
    },
    "nprogress": {
        "*.js": "",
        "*.css":""
    },
    "bootstrap-daterangepicker": {
        "daterangepicker.js": "",
        "daterangepicker.css":"",
        "moment.min.js":""
    },
    "fastclick": {
        "lib/**/*":""
    },
    "chart.js": {
        "dist/*.js":""
    },
    "jquery-sparkline": {
        "*.js":""
    },
    "flot": {
        "*.js":""
    },
    "flot-orderbars": {
        "js/**/*":""
    },
    "flot-spline": {
        "js/**/*": ""
    },
    "flot.curvedlines": {
        "*.js/":""
    },
    "datejs": {
        "build/date.js":""
    },
    "moment": {
        "min/**/*":""
    },
    //"notifyjs": {
    //    "dist/*":""
    //},
    "bootbox": {
        "*.js":""
    },
    "animate.css": {
        "*.css":""
    },
    "jquery-validation": {
        "dist/**/*":""
    },
    "jquery-validation-unobtrusive": {
        "dist/*":""
    },
    "mustache": {
        "*.js":""
    },
    "twbs-pagination": {
        "*.js":""
    },
    "centit.easyui": {
        "**/*":""
    }
};
var templates = {
    "gentelella": {
        "/build/**/*":"build",
        "/src/**/*": "src",
        "/production/images/*":"production"
    }
};

//Copy Lib to project
gulp.task("CopyLibrary", function () {
    var streams = [];
    for (var prop in nodeLibs) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in nodeLibs[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/vendor/" + prop + "/" + nodeLibs[prop][itemProp])));
        }
    }
});

//Copy style and js from template to project
gulp.task("CopyForTemplates", function() {
    var streams = [];
    for (var template in templates) {
        console.log("Coping style and js from Template " + template);
        for (var itemTemplate in templates[template]) {
            streams.push(gulp.src("node_modules/" + template + "/" + itemTemplate)
                .pipe(gulp.dest("wwwroot/vendor/" + template + "/" +  templates[template][itemTemplate])));
        }
    }
});

gulp.task("default", function (callback) {
    return runSequence(
        "CopyForTemplates",
        "CopyLibrary",
        callback);
});