(ns guestbook.core
  (:require [reagent.core :as r]
            [reagent.dom :as dom]))

  
(defn message-form []
  (let [fields (r/atom {})]
    (fn []
      [:div
       [:div.field
        [:label.label {:for :name} "Name"]
        [:input.input {:type      :text
                       :name      :name
                       :value     (:name @fields)
                       :on-change #(swap! fields assoc :name (-> %
                                                                 .-target
                                                                 .-value))}]]
       [:div.field
        [:label.label {:for :message} "Message"]
        [:textarea.textarea {:name      :message
                             :value     (:message @fields)
                             :on-change #(swap! fields assoc :message (-> %
                                                                          .-target
                                                                          .-value))}]]
       [:p "Name: " (:name @fields)]
       [:p "Message " (:message @fields)]
       [:input.button.is-primary {:type :submit
                                  :value "comment"}]])))


(defn home []
  [:div.content
   [:div.columns.is-centered
    [:div.column.is-two-thirds
     [:div.columns
      [:div.column
       [message-form]]]]]])


(dom/render [home] (.getElementById js/document "content"))