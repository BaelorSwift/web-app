/// <binding BeforeBuild='sass' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
	grunt.initConfig({
		bower: {
			install: {
				options: {
					targetDir: "wwwroot/lib",
					layout: "byComponent",
					cleanTargetDir: true
				}
			}
		},
		sass: {
			dist: {
				options: {
					style: 'expanded'
				},
				files: {
					'wwwroot/css/site.css': 'Assets/Sass/main.scss'
				}
			}
		}
	});

	grunt.loadNpmTasks("grunt-bower-task");
	grunt.loadNpmTasks("grunt-contrib-sass");
	grunt.registerTask("default", [
		"bower:install",
		"sass"
	]);
};