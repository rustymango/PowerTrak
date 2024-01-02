![PowerTrak](https://github.com/rustymango/PowerTrak/assets/117687423/b49392f7-37b0-4a7c-bc71-41dafc583919)
## Project Description
PowerTrak is an app designed to help powerlifters/weightlifters analyze and improve their lifts! PowerTrak uses Computer Vision to identify the barbell, track barbell velocity and timing during the eccentric, pause, and concentric portion of one's lifts! Currently, PowerTrak is not hosted on a server; however, there are plans to do so in the near future. I accept video requests through an external source, then analysis results will be sent back ASAP at a later time -- feel free to shoot me a message if you're interested! Alternatively, the app can be downloaded and used on the user's computer.

## Here are some videos that have been analyzed for our first users!

### Comp Bench 4 Reps - Yellow                     Pause Squat 3 Reps - Green
![Comp Bench 4 Reps - Yellow](https://github.com/rustymango/PowerTrak/assets/117687423/1929b31c-04c6-4d14-b0f8-26733be2bc2d) ![Pause Squat 3 Reps - Green](https://github.com/rustymango/PowerTrak/assets/117687423/082f0c2f-c8a0-4fab-a400-643bd8d8070e)

### Comp Bench 1 Rep - Blue                        Comp Bench 1 Rep - Yellow
https://github.com/rustymango/PowerTrak/assets/117687423/cccf15d5-19f6-42a6-ae71-15f02ad99826
https://github.com/rustymango/PowerTrak/assets/117687423/e626a1e6-d927-4129-a6e2-8cb870354382

### Comp Bench 1 Rep - Yellow                      Comp Bench 1 Rep - Red
https://github.com/rustymango/PowerTrak/assets/117687423/ff289574-cca2-4748-a1e2-14c24cb164d4
https://github.com/rustymango/PowerTrak/assets/117687423/c880b086-ee60-401c-9ab6-f1d7f083cd92

PowerTrak first filters out the pixels from the plate colour specified in the UI. It then tracks the time spent reaching the bottom of the eccentric portion of the lift and calculates a pause time based on time spent without vertical velocity.
