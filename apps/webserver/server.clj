(ns webserver.server
  "See: https://github.com/clojure-cookbook/clojure-cookbook/blob/master/05_network-io/5-10_tcp-server.asciidoc"
  (:require [clojure.java.io :as io])
  (:import [java.net ServerSocket]))

(defn receive [socket]
  (.readLine (io/reader socket)))

(defn respond [socket msg]
  (let [writer (io/writer socket)]
    (.write writer msg)
    (.flush writer)))

(defn serve [port handler]
  (with-open [server-socket (ServerSocket. port)
              socket        (.accept server-socket)]
    (let [request  (receive socket)
          response (handler request)]
      (respond socket response))))

(defn serve-continuously [port handler]
  (let [keep-serving? (atom true)]
    (future
      (with-open [server-socket (ServerSocket. port)]
        (while @keep-serving?
          (with-open [socket (.accept server-socket)]
            (let [request  (receive socket)
                  response (handler request)]
              (respond socket response))))))
    keep-serving?))

(comment
  (serve 8080 #(str "This is what I got: " %))
  (def keep-serving? (serve-continuously 9090 #(str "This is what I got: " %)))
  (reset! keep-serving? false)
  )