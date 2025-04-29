export function showMainContainer() {
	$(".js-preloader").fadeOut(800, function () {
		$(".js-main-container").fadeIn(800);

		setup_scrollreveal();
		setup_progress_bar_animation();
	});
}

function setup_progress_bar_animation() {
	const $animation_elements = $("[class*='a-']");
	const $window = $(window);

	$window.on('scroll resize', function () {
		const window_height = $window.height();
		const window_top_position = $window.scrollTop();
		const window_bottom_position = (window_top_position + window_height);

		$.each($animation_elements, function () {
			const $element = $(this);
			const element_height = $element.outerHeight();
			const element_top_position = $element.offset().top;
			const element_bottom_position = (element_top_position + element_height);

			// Check to see if this current container is within viewport
			if ((element_bottom_position >= window_top_position) &&
				(element_top_position <= window_bottom_position)) {
				$element.addClass('in-view');

				// Animate progress bar
				if ($element.hasClass('a-progress-bar')) {
					$element.css('width', $element.attr('data-percent') + '%');
				}
			}
			//else {
			//    $element.removeClass('in-view');
			//}
		});
	});

	$window.trigger('scroll');
}

function setup_scrollreveal() {
	if (typeof ScrollReveal !== 'undefined' && typeof ScrollReveal === "function") {
		window.sr = ScrollReveal();

		const default_config = {
			duration: 500,
			delay: 0,
			easing: 'ease',
			scale: 1,
			mobile: false
		};
		const header_config = $.extend(false, default_config, {
			duration: 1200,
			delay: 700
		});
		const footer_config = $.extend(false, default_config, {
			duration: 1500,
			distance: 0,
			viewOffset: { top: 0, right: 0, bottom: 100, left: 0 }
		});

		const default_delay = 175;

		sr.reveal('.a-header', header_config, default_delay);
		sr.reveal('.a-footer', footer_config, default_delay);
	}
}
