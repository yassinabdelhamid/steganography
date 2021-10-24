// file validation encode
function fileValidationDecode() {

    let fileInput = document.getElementById('decode');
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
//     let uploadField = document.getElementById("decode");

// uploadField.onchange = function() {
//     if(this.files[0].size > 307200){
//        alert("Datei zu gross!");
//        this.value = "";
//     };
// };
// }



let decodeCanvas = document.getElementById('imageDecode');
let dctx = decodeCanvas.getContext('2d');
let decode = document.getElementById('decode');
    decode.addEventListener('change', handleImageDecode, false);



function handleImageDecode(e){
    console.log('Decoding Image');
    let readerToDecode = new FileReader();
    readerToDecode.onload = function(event){
        console.log('readerToDecode loaded');
        let imgToDecode = new Image();
        imgToDecode.onload = function(){
            console.log('imgToDecode loaded');
            decodeCanvas.width = imgToDecode.width;
            decodeCanvas.height = imgToDecode.height;
            dctx.drawImage(imgToDecode,0,0);
            let decodeData = dctx.getImageData(0, 0, decodeCanvas.width, decodeCanvas.height);
            for (let i = 0; i < decodeData.data.length; i += 4) {
                if (decodeData.data[i+1] % 10 == 7) {
                    decodeData.data[i] = 0;
                    decodeData.data[i+1] = 0;
                    decodeData.data[i+2] = 0;
                    decodeData.data[i+3] = 255;
                }
                else {
                    decodeData.data[i+3] = 0;
                }
            }
            dctx.putImageData(decodeData, 0, 0);
        };
        imgToDecode.src = event.target.result;
    };
    readerToDecode.readAsDataURL(e.target.files[0]);
}