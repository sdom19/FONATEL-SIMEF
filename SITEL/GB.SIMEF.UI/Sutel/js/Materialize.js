jQuery(document).ready(function() {
    jQuery('.carousel').carousel({
      fullWidth: false,
      indicators: true,
    });

    jQuery('.modal').modal();
    jQuery(".button-collapse").sideNav();

    jQuery('.side-nav li').click(() => {
      jQuery('side-nav').sideNav('hide');
    })

    console.log('Test');
  });