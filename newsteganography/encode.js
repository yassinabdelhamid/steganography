// file validation encode
function fileValidationEncode() {

    let fileInput = document.getElementById('encode');
    let filePath = fileInput.value;

    let allowedTypes = /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedTypes.exec(filePath)) {
        alert('Bitte wählen Sie eine dieser Dateien aus: jpeg, jpg, oder png! ');
        fileInput.value = '';
        return false;

    }
}

// file size validatoin
// function fileSizeValidation() {
//     let uploadField = document.getElementById("encode");

// uploadField.onchange = function() {
//     if(this.files[0].size > 307200){
//        alert("Datei zu gross!");
//        this.value = "";
//     };
// };
// }

// upload after text entry
function showImageUpload(){

    let element = document.getElementById("imgUpload");	
    element.style.visibility = "visible";	

    
    element = document.getElementById("textInput");
    element.style.visibility = "hidden";
    
    //Abstand wegbekommen?
    
}

// file validation decode
function fileValidationDecode() {

    let fileInput = document.getElementById('decode');
    let filePath = fileInput.value;

    let allowedTypes = /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedTypes.exec(filePath)) {
        alert('Bitte wählen Sie eine dieser Dateien aus: jpeg, jpg, oder png! ');
        fileInput.value = '';
        return false;
    } else {
        if (fileInput.files && fileInput.files[0]) {
            let readerEncode = new FileReader();
            readerEncode.onload = function (e) {
                document.getElementById('imagePreview').innerHTML = '<imgToEncode src="' + e.target.result + '"/>';
            };
            readerEncode.readAsDataURL(fileInput.files[0]);
        }
    }
}

let encode = document.getElementById('encode');
    encode.addEventListener('change', handleImageEncode, false);

let encodeCanvas = document.getElementById('encodeCanvas');

let ctx = encodeCanvas.getContext('2d');

let messageInput = document.getElementById('message');

let textCanvas = document.getElementById('textCanvas');

let tctx = textCanvas.getContext('2d');

function handleImageEncode(e){
    let readerEncode = new FileReader();
    readerEncode.onload = function(event){
        let imgToEncode = new Image();
        imgToEncode.onload = function(){
            encodeCanvas.width = imgToEncode.width;
            encodeCanvas.height = imgToEncode.height;
            textCanvas.width=imgToEncode.width;
            textCanvas.height=imgToEncode.height;
            tctx.font = "30px Arial";

      let messageText = (messageInput.value.length) ? messageInput.value : 'Error: Bitte erneut versuchen und zuerst die Geheimnachricht eingeben';
            tctx.fillText(messageText,10,50);
            ctx.drawImage(imgToEncode,0,0);
            let imgData = ctx.getImageData(0, 0, encodeCanvas.width, encodeCanvas.height);
            let textData = tctx.getImageData(0, 0, encodeCanvas.width, encodeCanvas.height);
            let pixelsInMsg = 0;
                pixelsOutMsg = 0;
            for (let i = 0; i < textData.data.length; i += 4) {
                if (textData.data[i+3] !== 0) {
                    if (imgData.data[i+1]%10 == 7) {
                    }
                    else if (imgData.data[i+1] > 247) {
                        imgData.data[i+1] = 247;
                    }
                    else {
                        while (imgData.data[i+1] % 10 != 7) {
                            imgData.data[i+1]++;
                        }
                    }
                    pixelsInMsg++;
                }
                else {
                    if (imgData.data[i+1]%10 == 7) {
                        imgData.data[i+1]--;
                    }
                    pixelsOutMsg++;
                }
            }
            console.log('pixels within message borders: '+pixelsInMsg);
            console.log('pixels outside of message borders: '+pixelsOutMsg);

            ctx.putImageData(imgData, 0, 0);
        };
        imgToEncode.src = event.target.result;
    };
    readerEncode.readAsDataURL(e.target.files[0]);
}

