cmake_minimum_required(VERSION 2.6)
project(oscmangler)

include_directories(${oscmangler_SOURCE_DIR}/include ${oscmangler_SOURCE_DIR} ${oscmangler_SOURCE_DIR}/include/Common /opt/vc/include /opt/vc/include/interface/vcos/pthreads /opt/vc/include/interface/vmcs_host/linux /usr/include/glib-2.0 /usr/lib/glib-2.0/include )

add_executable(oscmangler main.cpp)

install(TARGETS oscmangler RUNTIME DESTINATION bin)
