/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
var gulp = require('gulp');
var concat = require('gulp-concat');

var fs = require('fs');
var path = require('path');
var merge2 = require('merge2');
var del = require('del');
var uglify = require('gulp-uglify');
var sass = require('gulp-sass');
var rev = require('gulp-rev');
var replace = require('gulp-html-replace');
var bower = require('gulp-bower');

var paths = {
    bower: "./bower_components",
    wwwroot: "./wwwroot",
    clientApp: "./wwwroot/app",
    lib: "./wwwroot/libs",
    assets: "./wwwroot/assets",
    customSass: "./Styles",
    sharedLayouts: './Areas/UserAccount/Views/Shared'
};

gulp.task('bower', function () {
  return bower(paths.bower);
});

gulp.task('default', ['build']);

gulp.task('clean', ['clean:css','clean:fonts']);

gulp.task('clean:css', function() {
  del(paths.assets + '/**/*.css');
});

gulp.task('clean:fonts', function() {
  del(paths.assets + '/fonts');
});

gulp.task('watch', function () {
  return gulp.watch(paths.customSass + '/**/*.scss', ['build']);
});

gulp.task('css', ['bower', 'clean:css'], function () {
    return gulp.src(paths.customSass + '/**/*.scss')
		.pipe(sass({
//            outputStyle: 'compressed',
            includePaths: [
                paths.customSass,
                paths.bower + '/bootstrap-sass-official/assets/stylesheets',
                paths.bower + '/font-awesome/scss'
            ]
        }).on('error', sass.logError)
     )
		.pipe(rev())
		.pipe(gulp.dest(paths.assets));
});

gulp.task('fonts', ['bower', 'clean:fonts'], function () {
  return gulp.src(paths.bower + '/font-awesome/fonts/**.*')
    .pipe(gulp.dest(paths.assets + '/fonts'));
});


gulp.task('build', ['css', 'fonts'], function() {

  // get a list of all custom views used by IdentityServer3
  var customViews = fs.readdirSync(paths.clientApp).reverse().map(function (f) {
    return paths.clientApp + '/' + f;
  }).filter(function (f) {
    return (f.indexOf('.html') !== -1);
  });

  // get a list of all built files
	var files = fs.readdirSync(paths.assets).reverse().map(function(f) { return '/assets/' + f; });
	
  // filter list to just css files
	var cssFiles = files.filter(function(f) {
		return f.indexOf('.css') !== -1;
	});
	
  // filter list to just javascript files
	var jsFiles = files.filter(function(f) {
		return f.indexOf('.js') !== -1;
	});

  // inject into the main layout
	gulp.src(paths.sharedLayouts + '/_Layout.cshtml')
		.pipe(replace(
      {
        'css': cssFiles,
        'js': jsFiles
      },
      {
			  keepBlockTags: true 
      })
    )
		.pipe(gulp.dest(paths.sharedLayouts));

  // inject into the custom views
	gulp.src(customViews)
		.pipe(replace(
      {
        'css': cssFiles,
        'js': jsFiles
      },
      {
			  keepBlockTags: true 
      })
    )
		.pipe(gulp.dest(paths.clientApp));
});