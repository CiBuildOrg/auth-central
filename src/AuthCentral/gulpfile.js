/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
var gulp = require('gulp');
var concat = require('gulp-concat');

var fs = require('fs');
var merge2 = require('merge2');
var del = require('del');
var uglify = require('gulp-uglify');
var sass = require('gulp-sass');
var rev = require('gulp-rev');
var replace = require('gulp-html-replace');
var bower = require('gulp-bower');

var paths = {
    bower: "./bower_components/",
    lib: "./wwwroot/libs/",
		assets: "./wwwroot/assets/"
};

gulp.task('bower', function () {
    return bower()
      .pipe(gulp.dest(config.bowerDir)) });

gulp.task('icons', function() {     return gulp.src(config.bowerDir + '/fontawesome/fonts/**.*')       .pipe(gulp.dest(paths.assets + 'fonts')); 
});

gulp.task('default', ['watch']);

gulp.task('build', ['sass'], function() {
	return gulp.start('layout');
});

gulp.task('build:sass', ['sass'], function() {
	return gulp.start('layout');
});

gulp.task('clean:sass', function() {
  del(paths.assets + '**/*.css');
  return gulp.start('bower');
});

gulp.task('watch', ['watch:sass']);

gulp.task('watch:sass', function () {
	return gulp.watch('./Styles/**/*.scss', function() {
		return gulp.start('build:sass');
	});
});

gulp.task('sass', ['clean:sass'], function () {
    return gulp.src(['./Styles/main.scss', './Styles/vendor.scss'])
		.pipe(sass({
            loadPath: [
                './Styles',
                config.bowerDir + '/bootstrap-sass-official/assets/stylesheets',
                config.bowerDir + '/fontawesome/scss',
            ]
        })
     )
		.pipe(rev())
		.pipe(gulp.dest(paths.assets) + 'css');
});

gulp.task('layout', function() {
	var files = fs.readdirSync(paths.assets).reverse().map(function(f) { return '/assets/' + f; });
	
	var cssFiles = files.filter(function(f) {
		return f.indexOf('.css') !== -1;
	});
	
	var jsFiles = files.filter(function(f) {
		return f.indexOf('.js') !== -1;
	});
	
	return gulp.src('./Areas/UserAccount/Views/Shared/_Layout.cshtml')
		.pipe(replace({
			'css': cssFiles,
			'js': jsFiles
		}, { 
			keepBlockTags: true 
		}))
		.pipe(gulp.dest('./Areas/UserAccount/Views/Shared/'));
});