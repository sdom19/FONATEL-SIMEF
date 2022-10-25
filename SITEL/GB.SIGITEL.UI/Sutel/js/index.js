$(".thumbnail").click(function(){
  $(".card").show("slow");
});    
$(window).load(function() {
  $('.post-module').hover(function() {
    $(this).find('.description').stop().animate({
      height: "toggle",
      opacity: "toggle"
    }, 800);
  });
});

  

