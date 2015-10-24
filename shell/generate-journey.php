<?php
/**
 * Mercuriel - Switzerland
 * @author Matias Orlando <support at mercuriel.ch>
 * @copyright   Copyright (c) 2011-2015 Mercuriel (http://www.mercuriel.ch)
 * Date: 24.10.2015
 * Time: 14:22
 */
ini_set('display_errors', 1);
require "../app/Mage.php";
$app = Mage::app('');

print "Please set the interval (in days):";

$interval = (int)fgets(STDIN)*86400;

print 'Interval of '.($interval/86400)." days";

$productsData = array();

$salesModel = Mage::getModel("sales/order");

unlink('../unity_data.json');

$from = time()-(3600*24*365*2);
$to = $from + $interval;

print PHP_EOL;
print 'from : '.date('Y-m-d', $from).PHP_EOL;
print 'to : '.date('Y-m-d', $to).PHP_EOL;

while($from < time()) {

    $salesCollection = $salesModel->getCollection()
        ->addAttributeToFilter('created_at', array(
            'from' => date('Y-m-d', $from),
            'to' => date('Y-m-d', $to),
        ));

    foreach ($salesCollection as $order) {
        $items = $order->getAllVisibleItems();
        foreach ($items as $item) {
            $productsData[$item->getSku()] += $item->getQtyOrdered();
        };

        $data[$from.'-'.$to]['sales'] += $order->getGrandTotal();
        $data[$from.'-'.$to]['orders']++;

    }

    sort($productsData);

    $topProducts = array_slice($productsData, -5, 5);

    $i = 1;

    foreach ($topProducts as $product) {
        $data[$from.'-'.$to]['product_' . $i] = $product;
        $i++;
    }

    $unityData = '../unity_data.json';

    if($data != null){
        file_put_contents($unityData, json_encode($data), FILE_APPEND);
    }

    unset($data);
    unset($productsData);


    $from = $to;
    $to = $from + $interval;

}