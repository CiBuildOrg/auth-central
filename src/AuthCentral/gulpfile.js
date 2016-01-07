/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
var gulp = require('gulp');
var fs = require('fs');
var del = require('del');
var sass = require('gulp-sass');
var rev = require('gulp-rev');
var replace = require('gulp-html-replace');
var bower = require('gulp-bower');
var gutil = require('gulp-util');

var paths = {
    bower: "./bower_components",
    wwwroot: "./wwwroot",
    idSvrCustomViews: "./IdSvr/CustomViews",
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
    var isRelease = false;
    var outputStyle = 'nested';

    if(process.env.AUTH_CENTRAL_BUILD_ENV==='release') {
        isRelease = true;
        outputStyle = 'compressed';
    }

    var s = gulp.src(paths.customSass + '/**/*.scss')
    .pipe(sass({
            outputStyle: outputStyle,
            includePaths: [
                paths.customSass,
                paths.bower + '/bootstrap-sass-official/assets/stylesheets',
                paths.bower + '/font-awesome/scss'
            ]
        }).on('error', sass.logError)
     );
 
    if(isRelease) {
        gutil.log(gutil.colors.yellow("IMPORTANT: ") + 
                  "'" + gutil.colors.cyan('AUTH_CENTRAL_BUILD_ENV') + "' " + 
                  gutil.colors.green('is set for') + " '" +
                  gutil.colors.magenta('release') + "'" +
                  gutil.colors.green(': Versioning css files...'));

        s = s.pipe(rev());
    }
    else {
        gutil.log(gutil.colors.yellow("IMPORTANT:") +  " Set "  + 
                  "'" + gutil.colors.cyan('AUTH_CENTRAL_BUILD_ENV') + '=' + 
                  gutil.colors.magenta('release') + "' " +
                  'to version css files.');
    }

    return s.pipe(gulp.dest(paths.assets));
});


gulp.task('fonts', ['bower', 'clean:fonts'], function () {
  return gulp.src(paths.bower + '/font-awesome/fonts/**.*')
    .pipe(gulp.dest(paths.assets + '/fonts'));
});


gulp.task('build', ['css', 'fonts'], function() {
  // get a list of all custom views used by IdentityServer3
  var customViews = fs.readdirSync(paths.idSvrCustomViews).reverse().map(function (f) {
    return paths.idSvrCustomViews + '/' + f;
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

  gutil.log("Printing all built css files in `" + paths.assets + "`:");
  cssFiles.forEach(function(element) {
      gutil.log('\t' + element);
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
		.pipe(gulp.dest(paths.idSvrCustomViews));
});
