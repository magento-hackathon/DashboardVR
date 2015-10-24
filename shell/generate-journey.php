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

$salesModel=Mage::getModel("sales/order");
$salesCollection = $salesModel->getCollection();
$productsData = array();


foreach($salesCollection as $order)
{
    $items = $order->getAllVisibleItems();
    foreach($items as $item){
        $productsData[$item->getSku()] += $item->getQtyOrdered();
    };

    $data['sales'] += $order->getGrandTotal();
    $data['orders']++;

}

sort($productsData);

$topProducts = array_slice($productsData,-5, 5);

$i = 1;

foreach($topProducts as $product){
    $data['product_'.$i] =$product;
    $i++;
}

echo json_encode($data);