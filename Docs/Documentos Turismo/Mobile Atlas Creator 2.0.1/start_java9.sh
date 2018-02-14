#!/bin/sh

# If you see this file in an editor you may have to set the executable bit and execute it as script in a terminal

# This file will start the Mobile Atlas Creator with custom memory settings for
# the JVM. With the below settings the heap size (Available memory for the application)
# will range from 64 megabyte up to 1024 megabyte.

# --add-modules java.xml.bind is a required parameter for Java9

java -Xms64m -Xmx1024M --add-modules java.xml.bind -jar Mobile_Atlas_Creator.jar