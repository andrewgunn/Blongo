module.exports = function () {
    var config = {
        compatibility: [
            'last 2 versions',
            'ie >= 9'
        ],
        paths: {
            bower: {
                root: './bower_components'
            },
            src: {
                root: './src/Blongo'
            },
            dest: {
                root: './src/Blongo/wwwroot'
            }
        },
        uncss: {
            html: '**/*.html'
        }
    };

    config.paths.bower.anchorJs = config.paths.bower.root + '/anchor-js';
    config.paths.bower.bootstrap = config.paths.bower.root + '/bootstrap';
    config.paths.bower.clipboard = config.paths.bower.root + '/clipboard';
    config.paths.bower.fontawesome = config.paths.bower.root + '/fontawesome';
    config.paths.bower.highlightjs = config.paths.bower.root + '/highlightjs';
    config.paths.bower.html5shiv = config.paths.bower.root + '/html5shiv';
    config.paths.bower.jquery = config.paths.bower.root + '/jquery';
    config.paths.bower.pagedown = config.paths.bower.root + '/pagedown';
    config.paths.bower.respond = config.paths.bower.root + '/respond';
    config.paths.bower.tether = config.paths.bower.root + '/tether';
    config.paths.bower.jQueryValidate = config.paths.bower.root + '/jquery-validation';
    config.paths.bower.jQueryValidateUnobtrusive = config.paths.bower.root + '/jquery-validation-unobtrusive';

    config.paths.src.fonts = config.paths.src.root + '/Fonts';
    config.paths.src.icons = config.paths.src.root + '/Icons';
    config.paths.src.images = config.paths.src.root + '/Images';
    config.paths.src.scripts = config.paths.src.root + '/Scripts';
    config.paths.src.styles = config.paths.src.root + '/Styles';

    config.paths.dest.fonts = config.paths.dest.root + '/fonts';
    config.paths.dest.icons = config.paths.dest.root + '/icons';
    config.paths.dest.images = config.paths.dest.root + '/img';
    config.paths.dest.scripts = config.paths.dest.root + '/js';
    config.paths.dest.styles = config.paths.dest.root + '/css';

    return config;
};