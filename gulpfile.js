"use strict";

var config = require("./gulpfile.config")();
var del = require("del");
var fs = require("fs");
var gulp = require("gulp");
var mergeStream = require("merge-stream");
var runSequence = require("run-sequence");
var yargs = require("yargs");

var $ = require("gulp-load-plugins")({ lazy: true });

var production = !!(yargs.argv.production);

gulp.task("build",
    function(done) {
        return runSequence("clean",
            ["fonts", "icons", "images", "scripts", "styles"],
            function() {
                done();
            });
    });

gulp.task("default",
    function(done) {
        return runSequence("build",
            "watch",
            function() {
                done();
            });
    });

gulp.task("clean",
    function() {
        return del(config.paths.dest.root + "/*");
    });

gulp.task("icons",
    function() {
        return gulp.src(config.paths.src.icons + "/*.*")
            .pipe(gulp.dest(config.paths.dest.icons));
    });

gulp.task("images",
    function() {
        return gulp.src(config.paths.src.images + "/**/*")
            .pipe($.if(production,
                $.imagemin({
                    progressive: true
                })))
            .pipe(gulp.dest(config.paths.dest.images));
    });

gulp.task("fonts", ["fonts:font-awesome"]);

gulp.task("fonts:font-awesome",
    function() {
        return gulp.src(config.paths.bower.fontawesome + "/fonts/**/*.*")
            .pipe(gulp.dest(config.paths.dest.fonts));
    });

gulp.task("scripts",
[
    "scripts:application",
    "scripts:html5shiv",
    "scripts:jquery",
    "scripts:loadCSS",
    "scripts:respond",
    "scripts:vendor"
]);

gulp.task("scripts:application",
    function() {
        var javaScripts = gulp.src([
            config.paths.src.scripts + "/**/*.js",
            "!" + config.paths.src.scripts + "/LoadCSS.js",
            "!" + config.paths.src.scripts + "/Modernizr.js",
            "!" + config.paths.src.scripts + "/OnLoadCss.js"
        ]);

        var typeScripts = gulp.src(config.paths.src.scripts + "/**/*.ts")
            .pipe($.typescript({
                sortOutput: true
            }));

        return mergeStream(javaScripts, typeScripts)
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.babel())
            .pipe($.concat("app.js"))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("scripts:html5shiv",
    function() {
        return gulp.src(config.paths.bower.html5shiv + "/dist/html5shiv-printshiv.js")
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("scripts:jquery",
    function() {
        return gulp.src(config.paths.bower.jquery + "/dist/jquery.js")
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("scripts:loadCSS",
    function() {
        return gulp.src([
                config.paths.src.scripts + "/loadCSS.js",
                config.paths.src.scripts + "/onloadCSS.js"
            ])
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.concat("loadcss.js"))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("scripts:respond",
    function() {
        return gulp.src(config.paths.bower.respond + "/dest/respond.src.js")
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.rename("respond.js"))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("scripts:vendor",
    function() {
        return gulp.src([
                config.paths.bower.anchorJs + "/anchor.js",
                config.paths.bower.bootstrap + "/js/dist/collapse.js",
                config.paths.bower.bootstrap + "/js/dist/dropdown.js",
                config.paths.bower.bootstrap + "/js/dist/modal.js",
                config.paths.bower.tether + "/dist/js/tether.js",
                config.paths.bower.bootstrap + "/js/dist/tooltip.js",
                config.paths.bower.bootstrap + "/js/dist/util.js",
                config.paths.bower.clipboard + "/dist/clipboard.js",
                config.paths.bower.highlightjs + "/highlight.pack.js",
                config.paths.bower.jQueryValidate + "/dist/jquery.validate.js",
                config.paths.bower.jQueryValidateUnobtrusive + "/jquery.validate.unobtrusive.js",
                config.paths.bower.konamiJs + "/konami.js",
                config.paths.bower.pagedown + "/Markdown.Converter.js",
                config.paths.bower.pagedown + "/Markdown.Editor.js",
                config.paths.bower.pagedown + "/Markdown.Sanitizer.js"
            ])
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.concat("vendor.js"))
            .pipe($.if(production, $.uglify().on("error", error => { console.log(error); })))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.scripts));
    });

gulp.task("styles", ["styles:application"]);

gulp.task("styles:application",
    function() {
        return gulp.src([
                config.paths.src.styles + "/Application.scss",
                config.paths.bower.highlightjs + "/styles/zenburn.css"
            ])
            .pipe($.sourcemaps.init({ loadMaps: true }))
            .pipe($.sass({
                    includePaths: [config.paths.src.styles]
                })
                .on("error", $.sass.logError))
            .pipe($.concat("app.css"))
            .pipe($.autoprefixer({
                browsers: config.compatibility
            }))
            .pipe($.if(production, $.cssnano()))
            .pipe($.if(!production, $.sourcemaps.write()))
            .pipe(gulp.dest(config.paths.dest.styles));
    });

gulp.task("watch",
    function() {
        gulp.watch(config.paths.src.icons + "/**/*", ["icons"]);
        gulp.watch(config.paths.src.images + "/**/*", ["images"]);
        gulp.watch(config.paths.src.fonts + "/**/*", ["fonts"]);
        gulp.watch(config.paths.src.scripts + "/**/*", ["scripts"]);
        gulp.watch(config.paths.src.styles + "/**/*", ["styles"]);
    });

// WIP...
gulp.task("globalize-en",
    function() {
        var locale = "en";

        var jsons = [
            require("./bower_components/cldr-data/supplemental/likelySubtags.json"),
            require("./bower_components/cldr-data/main/" + locale + "/ca-gregorian.json"),
            require("./bower_components/cldr-data/main/" + locale + "/timeZoneNames.json"),
            require("./bower_components/cldr-data/supplemental/timeData.json"),
            require("./bower_components/cldr-data/supplemental/weekData.json"),
            require("./bower_components/cldr-data/main/" + locale + "/numbers.json"),
            require("./bower_components/cldr-data/supplemental/numberingSystems.json")
        ];

        var javaScript = "Globalize.load(" +
            jsons.map(function(json) { return JSON.stringify(json); }).join(", ") +
            ");";

        return fs.writeFile("./globalize." + locale + ".js",
            javaScript,
            function(error) {
                if (error) {
                    console.log(error);
                } else {
                    console.log("The file was created!");
                }
            });
    });