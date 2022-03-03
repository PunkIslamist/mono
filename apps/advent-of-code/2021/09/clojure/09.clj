(ns advent-of-code.2021.05.clojure.05
  (:require [clojure.string :as string]
            [punkislamist.core :as pi]
            [clojure.set :as set]))


(defn neighbours [[dim-x dim-y] idx]
  (let [above         (- idx dim-x)
        right         (inc idx)
        below         (+ idx dim-x)
        left          (dec idx)
        column        (mod idx dim-x)
        right-border? (= column (dec dim-x))
        left-border?  (= column 0)
        top-border?   (< above 0)
        bot-border?   (>= below (* dim-x dim-y))]
    (cond-> []
      (not top-border?)   (conj above)
      (not right-border?) (conj right)
      (not bot-border?)   (conj below)
      (not left-border?)  (conj left))))


(defn paths
  ([go-there? use-result? board start]
   (paths go-there? use-result? board #{start} [start]))

  ([go-there? use-result? board visited path]
   (let [{:keys [dimensions points]} board
         pos        (peek path)
         neighbours (neighbours dimensions pos)
         unvisited  (-> neighbours
                        (set)
                        (set/difference visited))
         go-to      (filter #(go-there? (points %) (points pos)) unvisited)]
     (if (not-empty go-to)
       (mapcat
        (fn [idx] (paths go-there? use-result? board (conj visited idx) (conj path idx)))
        go-to)
       (if (use-result? (map points neighbours) (points pos))
         (list path)
         (empty list))))))


(defn all-neighbours? [pred]
  (fn [neighbours pos]
    (every? (fn [n] (pred n pos)) neighbours)))


(defn to-lower [board start]
  (paths (fnil <= 10) (all-neighbours? (fnil >= 10)) board start))


(defn to-higher [board start]
  (paths (fnil >= -1) (all-neighbours? (fnil <= -1)) board start))


(defn low-point? [{:keys [points dimensions]} point]
  (let [neighbour-vals (->> (neighbours dimensions point)
                            (map points)
                            (filter (complement nil?)))
        point-val      (points point)]
    (if (every? (partial < point-val) neighbour-vals)
      point
      false)))


(defn mark-visited [board points]
  (let [initial (transient (:points board))]
    (->> points
         (reduce
          (fn [coll key] (assoc! coll key nil))
          initial)
         (persistent!)
         (assoc board :points))))


(defn basin [board idx]
    (->> (to-higher board idx)
         (apply concat)
         (set)
         (filter #(not= 9 ((:points board) %)))))


(defn step [{:keys [low-points indices board basins]}]
  (let [lower   (->> (first indices)
                     (to-lower board)
                     (map peek)
                     (set))
        lowest  (filter (partial low-point? board) lower)
        to-mark (->> lower
                     (mapcat (partial to-higher board))
                     (into #{} cat))
        basins'  (map (partial basin board) lowest)]
    {:low-points (concat low-points lowest)
     :basins     (concat basins basins')
     :indices    (set/difference indices to-mark)
     :board      (mark-visited board to-mark)}))


(defn spatialize [board]
  (let [init {:low-points []
              :basins []
              :board board
              :indices (->> (:dimensions board)
                            (apply *)
                            (range 0)
                            (into (sorted-set)))}]
    (pi/take-upto
     (fn [state] (empty? (:indices state)))
     (iterate step init))))


(defn parse-line [line]
  (->> line
       (map str)
       (map pi/to-number)))


(def board
  (let [rows (->> (string/split-lines (slurp "apps/advent-of-code/2021/09/input.txt"))
                  (map parse-line))
        dim-x (count (first rows))
        dim-y (count rows)]
    {:dimensions [dim-x dim-y]
     :points (vec (apply concat rows))}))


(->> board
     (spatialize)
     (last)
     :low-points
     (map (:points board))
     (map inc)
     (apply +)) ; => 494

(->> board
     (spatialize)
     (last)
     :basins
     (sort-by count)
     (reverse)
     (take 3)
     (map count)
     (apply *)) ; => 1048128
