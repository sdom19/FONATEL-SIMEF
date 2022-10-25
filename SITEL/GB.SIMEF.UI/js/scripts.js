(function (jQuery, document) {

	// get tallest tab__content element
	let height = -1;

	jQuery('.tab__content').each(function () {
		height = height > jQuery(this).outerHeight() ? height : jQuery(this).outerHeight();
		jQuery(this).css('position', 'absolute');
	});

	// set height of tabs + top offset
	jQuery('[data-tabs]').css('min-height', height + 40 + 'px');

}(jQuery, document));

function scroll_to_class(element_class, removed_height) {
	var scroll_to = jQuery(element_class).offset().top - removed_height;
	if (jQuery(window).scrollTop() != scroll_to) {
		jQuery('html, body').stop().animate({
			scrollTop: scroll_to
		}, 0);
	}
}

function bar_progress(progress_line_object, direction) {
	var number_of_steps = progress_line_object.data('number-of-steps');
	var now_value = progress_line_object.data('now-value');
	var new_value = 0;
	if (direction == 'right') {
		new_value = now_value + (100 / number_of_steps);
	} else if (direction == 'left') {
		new_value = now_value - (100 / number_of_steps);
	}
	progress_line_object.attr('style', 'width: ' + new_value + '%;').data('now-value', new_value);
}



jQuery(document).ready(function () {
	jQuery('br', '#sidebar-wrapper').remove();
	jQuery('.field').css('background-color', '#f2f2f2');

	
	// jQuery('.carousel').carousel({
	// 	fullWidth: false,
	// 	indicators: true,
	// });

	
	// console.log('Test');
	jQuery("#block-delta-blocks-messages").hide();
	
	jQuery(".sidebar-nav").css("padding-left", "0px");
	jQuery("#region-content").removeClass("grid-8");
	jQuery("#region-social").hide();
	jQuery("#page").hide();
	jQuery("#section-footer").hide();
	jQuery("#region-header-second").hide();
	jQuery("#zone-content-wrapper").css("max-width", "100%");
	jQuery("#zone-content").css("max-width", "100%");

	jQuery(".def-btn").click(function () {
		changeTipoIndicador(jQuery(this).text());
	});



	/*
	    Fullscreen background
	*/
	// jQuery.backstretch("assets/img/backgrounds/1.jpg");

	jQuery('#top-navbar-1').on('shown.bs.collapse', function () {
		jQuery.backstretch("resize");
	});
	jQuery('#top-navbar-1').on('hidden.bs.collapse', function () {
		jQuery.backstretch("resize");
	});

	/*
	    Form
	*/
	jQuery('.f1 fieldset:first').fadeIn('slow');

	jQuery('.f1 input[type="text"], .f1 input[type="password"], .f1 textarea').on('focus', function () {
		jQuery(this).removeClass('input-error');
	});

	// next step
	jQuery('.f1 .btn-next').on('click', function () {
		var parent_fieldset = jQuery(this).parents('fieldset');
		var next_step = true;
		// navigation steps / progress steps
		var current_active_step = jQuery(this).parents('.f1').find('.f1-step.active');
		var progress_line = jQuery(this).parents('.f1').find('.f1-progress-line');

		// fields validation
		parent_fieldset.find('input[type="text"], input[type="password"], textarea').each(function () {
			if (jQuery(this).val() == "") {
				jQuery(this).addClass('input-error');
				next_step = false;
			} else {
				jQuery(this).removeClass('input-error');
			}
		});
		// fields validation

		if (next_step) {
			parent_fieldset.fadeOut(400, function () {
				// change icons
				current_active_step.removeClass('active').addClass('activated').next().addClass('active');
				// progress bar
				bar_progress(progress_line, 'right');
				// show next step
				jQuery(this).next().fadeIn();
				// scroll window to beginning of the form
				scroll_to_class(jQuery('.f1'), 20);
			});
		}

	});

	// previous step
	jQuery('.f1 .btn-previous').on('click', function () {
		// navigation steps / progress steps
		var current_active_step = jQuery(this).parents('.f1').find('.f1-step.active');
		var progress_line = jQuery(this).parents('.f1').find('.f1-progress-line');

		jQuery(this).parents('fieldset').fadeOut(400, function () {
			// change icons
			current_active_step.removeClass('active').prev().removeClass('activated').addClass('active');
			// progress bar
			bar_progress(progress_line, 'left');
			// show previous step
			jQuery(this).prev().fadeIn();
			// scroll window to beginning of the form
			scroll_to_class(jQuery('.f1'), 20);
		});
	});

	// submit
	jQuery('.f1').on('submit', function (e) {

		// fields validation
		jQuery(this).find('input[type="text"], input[type="password"], textarea').each(function () {
			if (jQuery(this).val() == "") {
				e.preventDefault();
				jQuery(this).addClass('input-error');
			} else {
				jQuery(this).removeClass('input-error');
			}
		});
		// fields validation

	});

	



});