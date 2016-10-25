cmake_minimum_required(VERSION 2.6)
project(dataorchestrator)

include_directories(${dataorchestrator_SOURCE_DIR}/include ${dataorchestrator_SOURCE_DIR} ${dataorchestrator_SOURCE_DIR}/include/Common /opt/vc/include /opt/vc/include/interface/vcos/pthreads /opt/vc/include/interface/vmcs_host/linux /usr/include/glib-2.0 /usr/lib/glib-2.0/include )

add_executable(dataorchestrator main.cpp)

install(TARGETS dataorchestrator RUNTIME DESTINATION bin)
