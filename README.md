
![Aura - Volumetric Lighting for Unity](https://i.imgur.com/VDDo0aX.png)

## Aura - Volumetric Lighting for Unity

----------

#### Please read this document before using this solution.
##### An extended version of this document can be found as the "Starting Guide.pdf".

----------
## About Aura

Aura is an open source volumetric lighting solution for Unity. 
The development is hosted on GitHub and the latest package is published on the Asset Store.

Aura simulates the scattering of the light in the environmental medium and the illumination of micro-particles that are present in this environment but invisible to the eye/camera.
This effect is also called “volumetric fog”.

![The directional light is scattered into the air and illuminates the invisible micro-particles. ](https://i.imgur.com/fW02LXF.png)

Aura uses a globalist approach that makes every lights and injection volumes interconnected. This means that when you locally modify the environment somewhere, it will automatically affect all the lights and injection volumes. No boring and redundant per-instance setup ...

Also, all lights and injection volumes are dynamic and then, can be created, modified or destroyed at runtime.

Aura packs a bunch of features such as full lighting support, injection volumes and particles illumination.

[Click here to view the recap of all the features of Aura (on YouTube).
![Click here to view the recap of all the features of Aura (on YouTube)](http://img.youtube.com/vi/PCrc2cWDX1E/0.jpg)](http://www.youtube.com/watch?v=PCrc2cWDX1E "")

----------
## Overview

Aura relies on Bart Wronski ([@BartWronsk](http://twitter.com/BartWronsk))’s [Presentation](https://bartwronski.files.wordpress.com/2014/08/bwronski_volumetric_fog_siggraph2014.pdf) and particularly the idea that the volumetric lighting could be solely computed in the frustum and pre-integrated in a 3D texture.

Starting from that, 7 additional points were targeted :
 - Dynamic quality *(not fixed ones)*
 - Ease of use *(not hours of integration or many parameters to struggle with)*
 - Make it cascaded *(for further range) -> Not implemented yet but bases are set*
 - Use Unity’s full lighting system *(not only one directional light)*
 - Use injection volumes *(locally modify the density, lighting and scattering)*
 - Illuminate Unity’s particles *(to have macro and micro particles volumetric lighting)*
 - Plug other processes *(so we could have volumetric GI, custom smoke, …)*

Here’s the schematic overview of Aura’s internal process :
![Schematic overview of Aura's internal process](https://i.imgur.com/bJjtP7f.png)

[Click here to visit the online API Documentation.](http://www.raphick.be/aura/documentation/)

**Please note that Aura is meant to be an evolving project. Importing updates might require re-tweaking, re-assigning Aura components or deleting the asset’s folder prior importing.**

----------
## Requirements 

Aura **strictly** requires full support of the following elements to work :
- RenderTextures (3D as well)
- Texture2DArrays
- ComputeShaders

Please verify that the support of these elements is not limited especially on lower platforms.

Historically, Texture2DArrays were introduced in Unity 5.4 which makes it the lowest version compatible with Aura.

However, multi-threaded ComputeShaders compilation was introduced in Unity 2017.2 which makes it the advised minimum version.

**The main ComputeShader has many variants (for performance purposes). This will lead its compilation, and therefore the import of the package in Unity, to take a long time (usually from 20 minutes to an hour!). 
PLEASE DO NOT KILL UNITY AND WAIT UNTIL THE IMPORT IS DONE.
If you did kill Unity, or experienced any trouble, during importation, re-import the file Aura/Shaders/ComputeShaders/ComputeDataComputeShader.compute**

Also, please also understand that you might need a Technical Artist or a Graphics Programmer if you want to make Aura fit some special requirements. 

----------
## Acknowledgement

Aura uses the following works :
 - [Bartlomiej Wronski (@BartWronsk)](http://twitter.com/BartWronsk)’s presentation : [“Volumetric Fog : Unified compute shader based solution to atmospheric scattering”](https://bartwronski.files.wordpress.com/2014/08/bwronski_volumetric_fog_siggraph2014.pdf)
 - [Sebastien Hillaire (@SebHillaire)](http://twitter.com/SebHillaire)’s integration formula : [“Physically Based and Unified Volumetric Rendering in Frostbite”](https://www.slideshare.net/DICEStudio/physically-based-and-unified-volumetric-rendering-in-frostbite/26)
 - Ashima Arts’s 4D Simplex Noise : [https://github.com/ashima/webgl-noise](https://github.com/ashima/webgl-noise)

About the provided demo scene :
 - Marko Dabrovic’s Sponza scene : [http://hdri.cgtechniques.com/~sponza/files/](http://hdri.cgtechniques.com/~sponza/files/)
 - Unity Lab’s smoke spritesheet : [“VFX Image Sequences & Flipbooks”](https://labs.unity.com/article/free-vfx-image-sequences-flipbooks)

Please let me know if I forgot any reference.

----------
## Special Thanks

For their time and help, I would like to cheerfully thank :
 - [Bartlomiej Wronski (@BartWronsk)](https://twitter.com/BartWronsk)
 - [Florent Guinier (@FlorentGuinier)](https://twitter.com/FlorentGuinier)
 - [Sebastien Lagarde (@SebLagarde)](https://twitter.com/SebLagarde)
 - [Gil Damoiseaux (@Gaxil)](https://twitter.com/Gaxil)
 - All the people that helped me by testing Aura, and kept me motivated with their constructive feedbacks and their kind words.

----------
## License

Aura is released under MIT License.
Please refer to the license.txt file located in the Aura/License folder for the full license.

**TL;DR;**
 - You can sell and make money with your projects that use Aura (or any derived form)
 - You can do whatever you want with Aura but not sell Aura (or any derived form)
 - You must propagate this license
 - There is no warranty of any kind
 - Please consider crediting Aura (or any derived form) in your projects, with the provided logo if relevant

----------
## FAQ

### Why releasing Aura for free?
For several reasons :
 - Aura was actually never meant to be sold. It was a personal challenge I set to myself. When people asked me about its price, I was just answering that such a solution would be worth around 40$.
 - I am a self learner. Half of the knowledge I have is from dissecting sources from people that shared them. I wanted to give this opportunity back to the community. As a Teacher I really feel like this is a good deed.
 - I just want to make the Unity community thrive and make Unity better on my own level.

### Can we make donations?
I didn’t expect that, thank you!
You can click on the following button for making a donation via Paypal.

[![You can click on the following button for making a donation via Paypal.](https://www.paypalobjects.com/en_US/GB/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RFCCD4V4LLQ6U "")

### Will Aura’s development be continued?
Hopefully yes! At least until Unity drops the legacy lighting system. Furthermore, I still have a bunch of feature ideas to implement, fixes and todos.

### Will Aura support the Scriptable Render Pipeline?
Anyhow, all this work cannot stop with the end of the legacy pipeline. So yes, my plan is to make it work with the SRP for the long term continuation. It may however be part of another branch/repository.

### Does Aura work on Unity xxx?
The technical requirement for Aura are specified in the Requirements section.
The minimum version possible for Aura is Unity 5.4 but the release is targeted for 2017.2.

### Does Aura support VR?
I couldn’t tell. I didn’t do any specific work to that end as I don’t have the hardware to test..

### Why did it take so long?
Aura was a personal project, a challenge for myself. It was developed during my free time. 
I am a full-time Technical Art Teacher and aside, we released our game (Outcast Second Contact) a few months ago. This, combined with my family life gives a pretty good idea of the amount of time I was able to manage for Aura.

### How can we help you developing Aura?
There are many ways you can help me facilitating the development of Aura :
 - You can spread the words about Aura.
 - You can branch the repo, improve the code, develop new features, fix bugs, ... 
I’ll merge with the Master if relevant.
 - You can share your work made with Aura (on my Twitter) or tips using Aura.
 - You can make a demo scene and send it to me. (Make sure to own the content of it so I can publish it in the package)
 - You can make a donation, like previously said.
 - Anything that can help making Aura a great project!

### How can I keep updated about Aura?
Two sure possibilities :
 - [Follow me on Twitter (@RaphErnaelsten)](https://twitter.com/RaphErnaelsten)
 - Wait for the updates of the package on the Asset Store.

----------
## Contact

[Feel free to contact me : @RaphErnaelsten](https://twitter.com/RaphErnaelsten)

![View Counter](https://s01.flagcounter.com/mini/sfjh/bg_FFFFFF/txt_000000/border_FFFFFF/flags_0/)
