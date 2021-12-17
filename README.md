# foto-app
In order to ru the app

1. Clone the app onto your computer
2. Ensure you have Docker client running on your computer
3. From the root of the app, type docker-compose up -d. This will run the app in detached from.
4. The above command should pull down and start two images for for the web and another for sql server database
5. The app should be running on your localhost:8000/api/foto.
6. You can check the api end-points with swagger which comes installed as part of the app and runs at localhost:8080/swagger/index.html

There are two end-points of interest to you.
1. /api/foto/createfoto
  This is used to upload any pictures taken. It takes the foto and stores it in the images folder under webroot directory.
2. /api/foto/getfotos
  Returns a json list of uploaded photos from the system
3. /api/foto/getfoto
  This is used to retrieve pictures in the requested sizes by the user.
  It takes in as parameters, the id of the photo (Guid), the width and height (all ints).
  It then calls the ResizeImage(foto, width, height) to resize the image to the parameter size before sending it to the 
  retriever. This returns a byte array, which can be converted to image

I didn't conclude with hooking up and securing the Api end-points. Code is all written by testing not done
I have not implemented a test project yet. To be included and updated

BBV
