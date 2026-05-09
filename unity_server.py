#!/usr/bin/env python3
import http.server
import socketserver
import os
from urllib.parse import unquote

class UnityWebGLHandler(http.server.SimpleHTTPRequestHandler):
    def end_headers(self):
        # Handle pre-compressed Unity WebGL files
        if self.path.endswith('.gz'):
            # Determine the actual content type based on the original file extension
            if '.data.gz' in self.path:
                self.send_header('Content-Type', 'application/octet-stream')
            elif '.wasm.gz' in self.path:
                self.send_header('Content-Type', 'application/wasm')
            elif '.js.gz' in self.path:
                self.send_header('Content-Type', 'application/javascript')
            # Add the Content-Encoding header for gzip
            self.send_header('Content-Encoding', 'gzip')
        http.server.SimpleHTTPRequestHandler.end_headers(self)

PORT = 8080
os.chdir('/workspace/w6')

with socketserver.TCPServer(("", PORT), UnityWebGLHandler) as httpd:
    print(f"Serving Unity WebGL at http://localhost:{PORT}")
    httpd.serve_forever()
