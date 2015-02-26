/// <binding />
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
			options: {
				sourceMap: true
			},
			dist: {
				files: {
					'wwwroot/css/site.css': 'Assets/Sass/site.scss'
				}
			}
		},
		typescript: {
			base: {
				src: ['Assets/Typescript/*.ts'],
				dest: 'wwwroot/js/',
				options: {
					basePath: 'Assets/Typescript',
					sourceMap: false,
					declaration: false
				}
			}
		}
	});

	grunt.loadNpmTasks("grunt-bower-task");
	grunt.loadNpmTasks("grunt-sass");
	grunt.loadNpmTasks("grunt-typescript");
	grunt.registerTask("default", [
		"bower:install",
		"sass",
		"typescript"
	]);
};