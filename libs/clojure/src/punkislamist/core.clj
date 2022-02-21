(ns punkislamist.core
  (:require [clojure.edn :as edn]))

(defn pwd [] (System/getProperty "user.dir"))


(defn to-number [s] (edn/read-string s))


(defn pivot [colls]
  (apply map vector colls))


(defn contained? [reference coll]
  (if (every? #(.contains reference %) coll)
    coll
    nil))
