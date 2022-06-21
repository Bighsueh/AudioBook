// let element_style;
let current_obj_path;
let current_name;
let current_code;
let current_type;

//取得素材庫資訊
function get_element_storage_data() {
  //uri
  let url = "/File/GetPageContent";

  $.ajax({
    url: url,
    method: "POST",
    data: {},
    success: function (res) {
      let image = res['image']; //影像
      let movie = res['movie']; //影片
      let audio = res['audio']; //音檔

      $.each(image, function (index, value) {
        let obj_path = value['obj_path']; //圖片的路徑:string
        let content_code = value['content_code']; //素材的編碼:string
        let content_name = value['content_name']; //素材的名稱:string

        let content_type = '圖片'; //素材的名稱:string

        let row = `<div class='col element_storage'>
        <figure class='figure'>
            <!-- content_type -->
            <p class='content_type' style='display: none;'>${content_type}</p>
            <!-- content_code -->
            <p class='content_code' style='display: none;'>${content_code}</p>
            <!-- obj_path -->
            <img src='${obj_path}' class='obj_path figure-img img-fluid rounded' alt='...'>
            <!-- content_name -->
            <figcaption class='figure-caption'>
                <p class='content_name'>${content_name}</p>
            </figcaption>
        </figure>
    </div>`;
        $('#tab_sound').append(row);
      });

      $.each(movie, function (index, value) {
        let obj_path = value['obj_path']; //影片的路徑:string
        let content_code = value['content_code']; //素材的編碼:string
        let content_name = value['content_name']; //素材的名稱:string

        let content_type = '影片';

        let row = `<div class='col element_storage'>
        <figure class='figure'>
            <!-- content_type -->
            <p class='content_type' style='display: none;'>${content_type}</p>
            <!-- content_code -->
            <p class='content_code' style='display: none;'>${content_code}</p>
            <!-- obj_path -->
            <img src='${obj_path}' class='obj_path figure-img img-fluid rounded' alt='...'>
            <!-- content_name -->
            <figcaption class='figure-caption'>
                <p class='content_name'>${content_name}</p>
            </figcaption>
        </figure>
    </div>`;
        $('#tab_sound').append(row);
      });

      $.each(audio, function (index, value) {
        let slow_path = value['slow_path']; //慢速音檔的路徑:string
        let english_path = value['english_path']; //全英音檔的路徑:string
        let bilingual_path = value['bilingual_path']; //中英音檔的路徑:string
        let content_code = value['content_code']; //素材的編碼
        let content_name = value['content_name'] //素材的名稱

        let content_type = '音檔';

        let row = `<div class='col element_storage'>
        <figure class='figure'>
            <!-- content_type -->
            <p class='content_type' style='display: none;'>${content_type}</p>
            <!-- content_code -->
            <p class='content_code' style='display: none;'>${content_code}</p>
            <!-- obj_path -->
            <img src='${obj_path}' class='obj_path figure-img img-fluid rounded' alt='...'>
            <!-- content_name -->
            <figcaption class='figure-caption'>
                <p class='content_name'>${content_name}</p>
            </figcaption>
        </figure>
    </div>`;
        $('#tab_sound').append(row);
      });

    },
    error: function (res) {
      console.log(res);
    },
  });
}



//取得頁面中各點擊框的位置
function get_page_element_data() {
  let url = "File/GetPageElementData";
  let page_id; //這裡要帶入page_id
  $.ajax({
    url: url,
    method: 'POST',
    data: {
      page_id: page_id,
    },
    success: function (res) {
      let page_elements = res['page_elements'];
      $.each(page_elements, function (index, value) {
        //素材名稱 :string
        let element_name = value['element_name'];
        //檔案類型 :string(audio,movie,image)
        let element_type = value['element_type'];
        //素材在頁面中的位置、大小設定 :string(position: absolute; left: 3.42%; top: 5.39%; width: 42.68%; height: 42.68%; z-index: 2; opacity:0;)
        let element_style = value['element_style'];
        //素材的代碼 :Guid
        let element_guid = value['element_guid'];
      });
    },
    error: function (res) {
      console.log(res);
    }
  })
}




//儲存頁面中各點擊框的位置
function store_page_element_data() {
  let url = "File/StorePageElementData";
  let page_id; //這裡要帶入page_id

  let elements = []; //將素材帶入

  $.each('.map-item', function (index, value) {
    //取得map item
    let map_item = $('.map-item').eq(index);
    //將各參數提取出來
    // let element_style = map_item.attr('style').trim();
    // let element_type = map_item.find('.element_type').text.trim();
    // let element_guid = map_item.find('.element_guid').text.trim();
    let element_style = map_item.attr('style').trim();
    let element_type = current_type;
    let element_guid = current_code;
    //整理匯出結果
    let result = {
      element_style: element_style,
      element_type: element_type,
      element_guid: element_guid,
    };
    elements.push(result);
  })

  $.ajax({
    url: url,
    method: 'POST',
    data: {
      page_id: page_id,
      elements: elements,

    },
    success: function (res) {
      if (res == 'success') {
        console.log('儲存成功');
      }
    },
    error: function (res) {
      console.log(res);
    }
  })
}



// 以下是對框框的設置
$(function () {
  mapr.init();
});


mapr = {
  count: 0,
  wrapper: null,
  widthPx: null,
  heightPx: null,
  image: null,
  imageWidth: null,
  ie: false,

  init: function () {
    mapr.loadImage();
    //set values
    mapr.image = $('#map-image');
    mapr.wrapper = $('#map-box');
    mapr.getImageSize();
    // mapr.addLink(mapr.count);
    //mapr.loadFromLocalStorage();
    mapr.testMap();

    //Attaching events
    $('.element_storage').on('click', function () {
      // current_obj_path=$(this).find('.obj_path').image();
      current_name = $(this).find('.content_name').text().trim();
      current_code = $(this).find('.content_code').text().trim();
      current_type = $(this).find('.content_type').text().trim();


      mapr.addLink(mapr.count);
    });

    $('#button-ie8').on('click', function () {
      mapr.ie8(this);
      mapr.writeHTML();
    });

    mapr.wrapper.on('click', '.map-item-close', function () {
      $(this).parent('div').remove();
    });

    mapr.wrapper.on('change, keyup', '.input-link, .input-title', function () {
      mapr.writeHTML();
    });

    mapr.wrapper.on('change, keyup', '.input-z-index', function () {
      $(this).parents('div').css('z-index', this.value);
      mapr.writeHTML();
    });

  },

  /*loadFromLocalStorage: function(){
    if(localStorage.getItem('mapHTML')) {
      mapr.wrapper.html(localStorage.getItem('mapHTML'));
      $('#code').html(localStorage.getItem('outputHTML'));
    }
  },*/

  loadImage: function () {
    //console.log(window.location.hash);
    $('#load-image').on('click', function () {
      console.log('yo');
      var imagePath = $('#image-path').val();
      console.log('image path:' + imagePath);
      mapr.image.attr('src', imagePath);
      mapr.getImageSize();
      //localStorage.setItem("imagePath",imagePath);
      return false;
    });
  },

  /*----------------------------------------------
  attachUI(i)
  attached the jquery UI events to the draggable
  elements dynamically. i is the number part 
  of the ID of the element. 
  ----------------------------------------------*/

  attachUI: function (i) {
    $('#map-item-' + i).draggable({
      containment: "parent",
      //scroll: false,
      drag: function () {
        mapr.updateCoords(this);
      },
      stop: function () {
        mapr.writeHTML();
        //localStorage.setItem('mapHTML',mapr.wrapper.html());
      }
    }).resizable({
      handles: "n, e, se, s, sw, w, nw",
      resize: function () {
        mapr.updateCoords(this);
      },
      stop: function () {
        mapr.writeHTML();
        //localStorage.setItem('mapHTML',mapr.wrapper.html());
      }
    });
  },

  getImageSize: function () {
    //check when image is loaded, set values and set map dimensions

    var newImg = new Image();
    newImg.onload = function () { mapr.setMapDimensions(); };
    newImg.src = mapr.image.attr('src');

    mapr.imageWidth = newImg.width;
  },

  setMapDimensions: function () {
    mapr.widthPx = mapr.wrapper.width();
    mapr.heightPx = mapr.wrapper.height();
  },

  updateCoords: function (el) {
    var $el = $(el),
      xPx = $el.position().left,
      xPercent = (Math.round((xPx / mapr.widthPx) * 10000)) / 100,
      yPx = $el.position().top,
      yPercent = (Math.round((yPx / mapr.heightPx) * 10000)) / 100,
      widthPx = $el.outerWidth(),
      widthPercent = (Math.round((widthPx / mapr.widthPx) * 10000)) / 100,
      heightPx = $el.outerHeight(),
      heightPercent = (Math.round((heightPx / mapr.heightPx) * 10000)) / 100;

    $el.find('.x-px').html(xPx + 'px');
    $el.find('.x-percent').html(xPercent + '%');
    $el.find('.y-px').html(yPx + 'px');
    $el.find('.y-percent').html(yPercent + '%');
    $el.find('.width-px').html(widthPx + 'px');
    $el.find('.width-percent').html(widthPercent + '%');
    $el.find('.height-px').html(heightPx + 'px');
    $el.find('.height-percent').html(heightPercent + '%');

    // element_style = xPx;
    // $el.find('.element_style').html(element_style);


    let audio = $el.find('.map-select').val();
    $el.attr('onclick', `playAudio('${audio}');`);
  },

  addLink: function () {
    mapr.count++;

    // var maprHTML =
    //   '<div class="map-item" id="map-item-' + mapr.count + '" >' +
    //   // '<div class="map-output-wrapper">' + 
    //   '<div class="map-item-close ui-icon ui-icon-close"></div>' +
    //   '<div class="map-output-box"><span class="map-output-label">x:</span> <span class="map-output x-px">20px</span> | <span class="map-output x-percent"></span></div>' +
    //   '<div class="map-output-box"><span class="map-output-label">y:</span> <span class="map-output y-px">20px</span> | <span class="map-output y-percent"></span></div>' +
    //   '<div class="map-output-box"><span class="map-output-label">width:</span> <span class="map-output width-px">150px</span> | <span class="map-output width-percent"></span></div>' +
    //   '<div class="map-output-box"><span class="map-output-label">height:</span> <span class="map-output height-px">150px</span> | <span class="map-output height-percent"></span></div>' +
    //   '<div class="map-output-box"><input class="map-input input-z-index" placeholder="Z" value="2" title="z-index"><input class="map-input input-link map-input-long" placeholder="Link" title="Link" value=""><input class="map-input input-title map-input-long" placeholder="Title" title="Link Title" value=""></div>' +
    //   // '</div>' + 
    //   '</div>';

    let mapr_id = `map-item-${mapr.count}`;
    let maprHTML_2 = `
        <div class="box-div map-item ui-draggable ui-resizable" id="${mapr_id}" onclick="">
        <p class="content_name" style="display: none;">${current_name}</p>
        <p class="content_code" style="display: none;">${current_code}</p>
        <p class="content_type" >${current_type}</p>
          <span class="map-output x-px" style="display: none;">20px</span>
          <span class="map-output y-px" style="display: none;">20px</span>
          <span class="map-output x-percent" style="display: none;"></span>
          <span class="map-output y-percent" style="display: none;"></span>
          <span class="map-output width-px" style="display: none;">150px</span>
          <span class="map-output height-px" style="display: none;">150px</span>
          <span class="map-output width-percent" style="display: none;"></span>
          <span class="map-output height-percent" style="display: none;"></span>
        </div>
      `;

    // mapr.wrapper.append(maprHTML);
    mapr.wrapper.append(maprHTML_2);
    //mapr.updateCoords('#map-item-' + mapr.count);
    mapr.attachUI(mapr.count);

  },
  writeHTML: function () {
    var id, x, y, w, h, z,
      $pre = $('#code'),
      mapHTML = '',
      link = '',
      title = '',
      ieCSS = '';

    if (mapr.ie === true) {
      ieCSS = 'filter: alpha(opacity=.1);';
    }

    $('.map-item').each(function () {
      id = this.id;
      x = $(this).find('.x-percent').text();
      y = $(this).find('.y-percent').text();
      w = $(this).find('.width-percent').text();
      h = $(this).find('.height-percent').text();
      z = $(this).find('.input-z-index').val();
      link = $(this).find('.input-link').val();
      title = $(this).find('.input-title').val();

      mapHTML = mapHTML + '<a href="' + link + '" title="' + title + '" style="position: absolute; left: ' + x + '; top: ' + y + ';  width: ' + w + ';  height: ' + h + ';  z-index: ' + z + ';' + ieCSS + '"></a>';
    });
    mapHTML = mapHTML.replace(/&/g, '&amp;');
    mapHTML = mapHTML.replace(/</g, '&lt;');
    mapHTML = mapHTML.replace(/>/g, '&gt;');

    $('#code').html(mapHTML);
    localStorage.setItem('outputHTML', mapHTML);
  },


  /*----------------------------------------------
  ie8()
  loads a clear.gif image in the map to 
  
  ----------------------------------------------*/

  ie8: function (el) {

    if (mapr.ie === false) {
      mapr.ie = true;
      $(el).addClass('active');
    }
    else {
      mapr.ie = false;
      $(el).removeClass('active');
    }
  },

  /*selectElementContents: function(el) {
    var range = document.createRange();
    range.selectNodeContents(el);
    var sel = window.getSelection();
    sel.removeAllRanges();
    sel.addRange(range);
  },*/

  /*----------------------------------------------
  testMap()
  used to test the image map links on the image 
  probably not going to be used for production.
  ----------------------------------------------*/

  testMap: function () {
    var mapHTML = '';
    $('#test-map').on('click', function () {
      mapHTML = $('#code').html();
      mapHTML = mapHTML.replace(/&lt;/g, '<');
      mapHTML = mapHTML.replace(/&gt;/g, '>');
      mapr.wrapper.append(mapHTML);
      return false;
    });
    $('#remove-test-map').on('click', function () {
      mapr.wrapper.find('a').remove();
    });
  }
};




