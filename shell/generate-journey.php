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

$from = time()-(3600*24*365*2);
$to = $from + $interval;

print PHP_EOL;
print 'from : '.date('Y-m-d', $from).PHP_EOL;
print 'to : '.date('Y-m-d', $to).PHP_EOL;

$unityData = '../unity_data.json';

unlink($unityData);
file_put_contents($unityData, '[', FILE_APPEND);

$jsonPrefix = '';

while($from < time()) {

    $data = array();
    $data['ts'] = $from.'-'.$to;

    $salesCollection = $salesModel->getCollection()
        ->addAttributeToFilter('created_at', array(
            'from' => date('Y-m-d', $from),
            'to' => date('Y-m-d', $to),
        ));
//->setPageSize(2);

    $productsDataOrdered = array();
    $productsData = array();

    foreach ($salesCollection as $order) {
        $items = $order->getAllVisibleItems();
        foreach ($items as $item) {
          
            if(!array_key_exists($item->getName(), $productsDataOrdered)) {
                $productsDataOrdered[$item->getName()] = 0;
            }
            
            if(!array_key_exists($item->getName(), $productsData)) {
                $productsData[$item->getName()] = array();
            }
          
            $productsDataOrdered[$item->getName()] += $item->getQtyOrdered();
            
            if(!array_key_exists('prize', $productsData[$item->getName()])) {
                $prize = floatval($item->getOriginalPrice());
                $productsData[$item->getName()]['prize'] = $prize;
            }
        };

        $data['sales'] += $order->getGrandTotal();
        $data['orders']++;
    }

    asort($productsDataOrdered);

    $topProducts = array_slice($productsDataOrdered, -5, 5, true);
//die(print_r($productsData, true));

    $data['products'] = array();

    foreach ($topProducts as $key => $product) {
        $prodData = $productsData[$key];
        $data['products'][] = array('name' => $key, 'sold' => $product, 'prize' => $prodData['prize']);
    }

    if($data != null){
        file_put_contents($unityData, $jsonPrefix . json_encode($data), FILE_APPEND);
    }

    unset($data);
    unset($productsData);

    $from = $to;
    $to = $from + $interval;

    $jsonPrefix = ',';

}

file_put_contents($unityData, ']', FILE_APPEND);