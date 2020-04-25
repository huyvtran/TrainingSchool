/*
*  Copyright (c) 2015 The WebRTC project authors. All Rights Reserved.
*
*  Use of this source code is governed by a BSD-style license
*  that can be found in the LICENSE file in the root of the source
*  tree.
*/

'use strict';

const videoElement = document.querySelector('video');
const audioInputSelect = document.querySelector('select#audioSource');
const videoSelect = document.querySelector('select#videoSource');
const selectors = [audioInputSelect, videoSelect];
var mixer;
var newAPI;

function gotDevices(deviceInfos) {
  // Handles being called several times to update labels. Preserve values.
  const values = selectors.map(select => select.value);
  selectors.forEach(select => {
    while (select.firstChild) {
      select.removeChild(select.firstChild);
    }
  });
  for (let i = 0; i !== deviceInfos.length; ++i) {
    const deviceInfo = deviceInfos[i];
    const option = document.createElement('option');
    option.value = deviceInfo.deviceId;
    if (deviceInfo.kind === 'audioinput') {
      option.text = deviceInfo.label || `microphone ${audioInputSelect.length + 1}`;
      audioInputSelect.appendChild(option);
    } else if (deviceInfo.kind === 'videoinput') {
      option.text = deviceInfo.label || `camera ${videoSelect.length + 1}`;
      videoSelect.appendChild(option);
    } else {
      //console.log('Some other kind of source/device: ', deviceInfo);
    }
  }
  
  const option = document.createElement('option');
  option.value = 'camera-screen';
    option.text='Camera + Screen';
    videoSelect.appendChild(option);
    
  const option2 = document.createElement('option');
  option2.value = 'only-screen';
    option2.text='Only Screen + Audio';
    videoSelect.appendChild(option2);
    
  selectors.forEach((select, selectorIndex) => {
    if (Array.prototype.slice.call(select.childNodes).some(n => n.value === values[selectorIndex])) {
      select.value = values[selectorIndex];
    }
  });
}

navigator.mediaDevices.enumerateDevices().then(gotDevices).catch(handleError);

// Attach audio output device to video element using device/sink ID.
function attachSinkId(element, sinkId) {
  if (typeof element.sinkId !== 'undefined') {
    element.setSinkId(sinkId)
      .then(() => {
        console.log(`Success, audio output device attached: ${sinkId}`);
      })
      .catch(error => {
        let errorMessage = error;
        if (error.name === 'SecurityError') {
          errorMessage = `You need to use HTTPS for selecting audio output device: ${error}`;
        }
        console.error(errorMessage);
        // Jump back to first output device in the list as it's the default.
      });
  } else {
    console.warn('Il tuo browser non supporta la sezione dei dispositivi audio/video');
  }
}


function gotStream(stream) {
  window.stream = stream; // make stream available to console
  videoElement.srcObject = stream;
  try{
		videoElement.srcObject = stream;
	} catch (error){
		videoElement.src = window.URL.createObjectURL(stream);
	}
  //videoElement.src = stream;
  // Refresh button list in case labels have become available
  return navigator.mediaDevices.enumerateDevices();
}

function handleError(error) {
  console.log('navigator.MediaDevices.getUserMedia error: ', error.message, error.name);
}

function getMixedCameraAndScreen() {
                if(navigator.getDisplayMedia) {
                    navigator.getDisplayMedia({video: true}).then(screenStream => {
                        afterScreenCaptured(screenStream);
                    });
                }
                else if(navigator.mediaDevices.getDisplayMedia) {
                    navigator.mediaDevices.getDisplayMedia({video: true}).then(screenStream => {
                        afterScreenCaptured(screenStream);
                    });
                }
                else {
                    alert('La registrazione del desktop non è supportata dal browser in uso.');
                }
            }
            
function afterScreenCaptured(screenStream) {
                navigator.mediaDevices.getUserMedia({
                    audio:true,video: true
                }).then(function(cameraStream) {
                    screenStream.fullcanvas = true;
                    screenStream.width = screen.width;
                    screenStream.height = screen.height;
                    screenStream.top = 0;
                    screenStream.left = screen.width-screenStream.width;
                    
                    cameraStream.width = parseInt((16 / 100) * screen.width);
                    cameraStream.height = parseInt((18 / 100) * screen.height);
                    cameraStream.top = screen.height-cameraStream.height-3;
                    cameraStream.left = 3;
                    

                    //fullCanvasRenderHandler(screenStream, 'Your Screen!');
                    //normalVideoRenderHandler(cameraStream, 'Your Camera!');

                    mixer = new MultiStreamsMixer([screenStream, cameraStream]);

                    mixer.frameInterval = 1;
                    mixer.startDrawingFrames();
                    
                    gum.srcObject = mixer.getMixedStream();

                    //updateMediaHTML('Mixed Screen+Camera!');

                    addStreamStopListener(screenStream, function() {
                        mixer.releaseStreams();
                        gum.pause();
                        //gum.src = null;

                        cameraStream.getTracks().forEach(function(track) {
                            track.stop();
                        });
                    });
                });
            }
            
    function getMixedAudioAndScreen() {
                if(navigator.getDisplayMedia) {
                    navigator.getDisplayMedia({video: true}).then(screenStream => {
                        afterAudioScreenCaptured(screenStream);
                    });
                }
                else if(navigator.mediaDevices.getDisplayMedia) {
                    navigator.mediaDevices.getDisplayMedia({video: true}).then(screenStream => {
                        afterAudioScreenCaptured(screenStream);
                    });
                }
                else {
                    alert('La registrazione del desktop non è supportata dal browser in uso.');
                }
            }
            
    function afterAudioScreenCaptured(screenStream) {
                navigator.mediaDevices.getUserMedia({
                    audio:true,video: false
                }).then(function(cameraStream) {
                    screenStream.fullcanvas = true;
                    screenStream.width = screen.width;
                    screenStream.height = screen.height;
                    screenStream.top = 0;
                    screenStream.left = screen.width-screenStream.width;
                    

                    mixer = new MultiStreamsMixer([screenStream, cameraStream]);

                    mixer.frameInterval = 1;
                    mixer.startDrawingFrames();
                    
                    gum.srcObject = mixer.getMixedStream();

                    //updateMediaHTML('Mixed Screen+Camera!');

                    addStreamStopListener(screenStream, function() {
                        mixer.releaseStreams();
                        gum.pause();
                        //gum.src = null;

                        cameraStream.getTracks().forEach(function(track) {
                            track.stop();
                        });
                    });
                });
            }
            
    function addStreamStopListener(stream, callback) {
                stream.addEventListener('ended', function() {
                    callback();
                    callback = function() {};
                }, false);
                stream.addEventListener('inactive', function() {
                    callback();
                    callback = function() {};
                }, false);
                stream.getTracks().forEach(function(track) {
                    track.addEventListener('ended', function() {
                        callback();
                        callback = function() {};
                    }, false);
                    track.addEventListener('inactive', function() {
                        callback();
                        callback = function() {};
                    }, false);
                });
            }
            
    
    

            
function getUserMediaSuccess() {


}
function handleError(error) {
  console.log('navigator.MediaDevices.getUserMedia error: ', error.message, error.name);
}
function startdevices() {
  if (window.stream) {
    window.stream.getTracks().forEach(track => {
      track.stop();
    });
  }
  const audioSource = audioInputSelect.value;
  const videoSource = videoSelect.value;
  
  if(videoSource === 'camera-screen') {
    getMixedCameraAndScreen();
    
   }
   else if(videoSource === 'only-screen') {
    getMixedAudioAndScreen();
    
   }
   else {
   const hasEchoCancellation = document.querySelector('#echoCancellation').checked;
  const constraints = {
    audio: {deviceId: audioSource ? {exact: audioSource} : undefined, echoCancellation: {exact: hasEchoCancellation}},
    video: {deviceId: videoSource ? {exact: videoSource} : undefined},
	
  };
  console.log('Using media constraints:', constraints);
  
  
   if(navigator.mediaDevices.getUserMedia)
	{
		navigator.mediaDevices.getUserMedia(constraints).then(gotStream).then(gotDevices).catch(handleError);
		//navigator.mediaDevices.getUserMedia(constraints).then(gotStream).catch(handleError);
		newAPI = false;
	}
    else if (navigator.getUserMedia)
    {
		navigator.getUserMedia(constraints).then(gotStream).then(gotDevices).catch(handleError);
        //navigator.getUserMedia(constraints, getUserMediaSuccess, handleError);
    }
    else
    {
        alert('Il tuo browser non supporta la registrazione della videocamera');
    }
    }
}

audioInputSelect.onchange = startdevices;
echoCancellation.onchange = startdevices;
videoSelect.onchange = startdevices;

startdevices();