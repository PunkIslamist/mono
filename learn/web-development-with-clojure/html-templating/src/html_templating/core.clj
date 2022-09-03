(ns html-templating.core
  (:require [selmer.parser :as selmer]))

(comment
  (selmer/render "Hello, {{name}}" {:name "Ding dong"})
  (selmer/render-file "hello.html" {:name "Amy"})
  (selmer/render-file
    "hello.html"
    {:items (range 3)})
  )
