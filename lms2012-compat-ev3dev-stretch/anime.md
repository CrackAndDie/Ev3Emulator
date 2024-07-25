## Changes I made to this shite that I probably need to revert in the future: 

### c_memory  

- changed open/write/read/close to fopen/fread/fwrite/fclose (check original and compare to this)
- these files probably should be opened as binary
- ~1475 line cMemoryGetMediaName commented func because of mounts
- ~4065 line commented memset - gave error and there was no the line if ev3sources
- some includes removed  

### c_sound  

- removed all pcm shite. it has to be binded with c# shite so c# shite would be playing the sounds