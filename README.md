<div style="text-align: center;">
  ![PowerTrak](https://github.com/rustymango/PowerTrak/assets/117687423/b49392f7-37b0-4a7c-bc71-41dafc583919)
</div>
## Project Description
PowerTrak is an app designed to help powerlifters/weightlifters analyze and improve their lifts! PowerTrak uses Computer Vision to identify the barbell, track barbell velocity and timing during the eccentric, pause, and concentric portion of one's lifts! Currently, PowerTrak is not hosted on a server; however, there are plans to do so in the near future. I accept video requests through an external source, then analysis results will be sent back ASAP at a later time -- feel free to shoot me a message if you're interested! Alternatively, the app can be downloaded and used on the user's computer.

## Here are some videos that have been analyzed for our first users!
### Comp Bench 4 Reps - Yellow                     
![bench yellow 4 rep](https://github.com/rustymango/PowerTrak/assets/117687423/ccb54ecd-4730-4bab-9e65-cc855bed357e) 

### Pause Squat 3 Reps - Green
![squat green pause 3 rep](https://github.com/rustymango/PowerTrak/assets/117687423/2b234daf-55b0-4bee-b1b9-9cef4090288a)

### Comp Bench 1 Rep - Yellow
![bench yellow cat](https://github.com/rustymango/PowerTrak/assets/117687423/7a0a1718-2501-4d50-8d0f-a70c175127bb)

### Comp Bench 1 Rep - Blue                        
![bench blue](https://github.com/rustymango/PowerTrak/assets/117687423/67157292-8292-4c29-90e7-b13dab23aa7f)

### Comp Bench 1 Rep - Yellow                      
![bench yellow denning](https://github.com/rustymango/PowerTrak/assets/117687423/d2a01ea5-4a1a-43d1-9e11-1493c57435ae)

### Comp Bench 1 Rep - Red
![bench red](https://github.com/rustymango/PowerTrak/assets/117687423/54136575-c69c-44fa-8fd4-a9515072b3c0)

PowerTrak first filters out the pixels from the plate colour specified in the UI. It then tracks the time spent reaching the bottom of the eccentric portion of the lift and calculates a pause time based on time spent without vertical velocity.
