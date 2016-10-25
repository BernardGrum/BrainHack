#include <iostream>
#include <stdio.h>      // standard input / output functions
#include <stdlib.h>
#include <string.h>     // string function definitions
#include <unistd.h>     // UNIX standard function definitions
#include <fcntl.h>      // File control definitions
#include <errno.h>      // Error number definitions
#include <termios.h>    // POSIX terminal control definitions

#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#include <math.h>
#include <string>
 
#define OSCPKT_OSTREAM_OUTPUT
#include "oscpkt/oscpkt.hh"
#include "oscpkt/udp.hh"

using std::cout;
using std::cerr;

using namespace oscpkt;
using namespace std;

const int BUFSIZE = 4000;
const int PORT_NUM = 12000;

string server;
string vlcserver;
string command;

char sendbuffer[53000];
#include <stdint.h>
#include <stdlib.h>

int main(int argc, char **argv) 
{
	
	oscpkt::UdpSocket sock; 
	server="192.168.2.121";
	vlcserver = "192.168.2.167";
 	vlcserver = "192.168.2.167";
	command = "pl_play";
	if( argc>1 ) server = argv[1];
	if( argc>2 ) vlcserver = argv[2];
	if( argc>3 ) command = argv[3];
	sock.connectTo(server.c_str(), PORT_NUM);
	
    char buffer[BUFSIZE];
    enum CONSTEXPR { MAX_REQUEST_LEN = 1024};
    char request[MAX_REQUEST_LEN];
    char request_template[] = "GET /requests/status.xml?command=%s HTTP/1.1\r\nHost: %s\r\nConnection: Close\r\nAuthorization: Basic %s\r\n\r\n";
    struct protoent *protoent;
    char hostname[255];
	strcpy(hostname, vlcserver.c_str());
    in_addr_t in_addr;
    int request_len;
    int socket_file_descriptor;
    ssize_t nbytes_total, nbytes_last;
    struct hostent *hostent;
    struct sockaddr_in sockaddr_in;
    unsigned short server_port = 9980;

	
//	UdpSocket sock; 
	sock.bindTo(PORT_NUM);
	if (!sock.isOk()) {
		cerr << "Error opening port " << PORT_NUM << ": " << sock.errorMessage() << "\n";
	} else {
		cout << "Server started, will listen to packets on port " << PORT_NUM << std::endl;
		PacketReader pr;
		PacketWriter pw;
			
		/* Build the socket. */
		
		protoent = getprotobyname("tcp");
		if (protoent == NULL) {
			perror("getprotobyname");
			exit(EXIT_FAILURE);
		}
		socket_file_descriptor = socket(AF_INET, SOCK_STREAM, protoent->p_proto);
		if (socket_file_descriptor == -1) {
			perror("socket");
			exit(EXIT_FAILURE);
		}
	//*
		hostent = gethostbyname(hostname);
		if (hostent == NULL) {
			fprintf(stderr, "error: gethostbyname(\"%s\")\n", hostname);
			exit(EXIT_FAILURE);
		}
		in_addr = inet_addr(inet_ntoa(*(struct in_addr*)*(hostent->h_addr_list)));
		if (in_addr == (in_addr_t)-1) {
			fprintf(stderr, "error: inet_addr(\"%s\")\n", *(hostent->h_addr_list));
			exit(EXIT_FAILURE);
		}
		sockaddr_in.sin_addr.s_addr = in_addr;
	/**/	
		sockaddr_in.sin_family = AF_INET;
		sockaddr_in.sin_port = htons(server_port);

		if (connect(socket_file_descriptor, (struct sockaddr*)&sockaddr_in, sizeof(sockaddr_in)) == -1) {
			perror("connect");
			exit(EXIT_FAILURE);
		}

		nbytes_total = 0;
		
		float timestamp;
		float vals[24];
		float val = 12.42;
		sprintf(sendbuffer, "%f;%f", val, val*4);
// 		nbytes_last = write(socket_file_descriptor, (const void*)sendbuffer, strlen(sendbuffer));
		
		while (sock.isOk()) {      
				if (sock.receiveNextPacket(30 /* timeout, in ms */)) {
					pr.init(sock.packetData(), sock.packetSize());
					oscpkt::Message *msg;
					while (pr.isOk() && (msg = pr.popMessage()) != 0) {
					int iarg;
					float val, val2, val3, pop;
	// 				cout << "Server: received msg " << *msg << " from " << sock.packetOrigin() << "\n";
					if (msg->match("/bci")
						.popFloat(timestamp)
						.popFloat(vals[0])
						.popFloat(vals[1])
						.popFloat(vals[2])
						.popFloat(vals[3])
						.popFloat(vals[4])
						.popFloat(vals[5])
						.popFloat(vals[6])
						.popFloat(vals[7])
						.popFloat(vals[8])
						.popFloat(vals[9])
						.popFloat(vals[10])
						.popFloat(vals[11])
						.popFloat(vals[12])
						.popFloat(vals[13])
						.popFloat(vals[14])
						.popFloat(vals[15])
						.popFloat(vals[16])
						.popFloat(vals[17])
						.popFloat(vals[18])
// 						.popFloat(vals[19])
						.isOkNoMoreArgs()) 
					{
	// 					cout << "Server: received /battery " << val << " from " << sock.packetOrigin() << "\n";
						
						char templat[1024];
						templat[0] = 0;
						strcat(templat,"%f");
						int limit = 190000;
						for( int i=0;i<16; i++) {
							if( vals[i]> limit) vals[i] = limit;
							vals[i] = fabs(vals[i])/limit;
							strcat(templat,";%f");
						}
						sprintf(sendbuffer, templat, timestamp, vals[0], vals[1], vals[2], vals[3], vals[4], vals[5], vals[6], vals[7], vals[8], vals[9], vals[10], vals[11], vals[12], vals[13], vals[14], vals[15]);
						printf(sendbuffer); printf("\n");
						nbytes_last = write(socket_file_descriptor, (const void*)sendbuffer, strlen(sendbuffer));
						
						if (nbytes_last == -1) {
							perror("write");
							exit(EXIT_FAILURE);
						}
						nbytes_total += nbytes_last;
					} else if (msg->match("/event").popFloat(val).popFloat(val2).popFloat(val3).isOkNoMoreArgs()) {
	// 					cout << "Server: received /battery " << val << " from " << sock.packetOrigin() << "\n";
						nbytes_last = write(socket_file_descriptor, request + nbytes_total, request_len - nbytes_total);
						if (nbytes_last == -1) {
							perror("write");
							exit(EXIT_FAILURE);
						}
						nbytes_total += nbytes_last;
					} else {
						cout << "Server: unhandled message: " << *msg << "\n";
					}
	// 				cout << "Server message: " << *msg << "\n";
				}
			}
		}
	}
/*
    while ((nbytes_total = read(socket_file_descriptor, buffer, BUFSIZE)) > 0) {
		continue;
    }
/**/
    if (nbytes_total == -1) {
        perror("read");
        exit(EXIT_FAILURE);
    }

    close(socket_file_descriptor);
/**/	
	return 0;
	
}
