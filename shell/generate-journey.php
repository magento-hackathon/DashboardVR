<?php
/**
 * Mercuriel - Switzerland
 * @author Matias Orlando <support at mercuriel.ch>
 * @copyright   Copyright (c) 2011-2015 Mercuriel (http://www.mercuriel.ch)
 * Date: 24.10.2015
 * Time: 14:22
 */
require "../Mage.php";
$app = Mage::app('');

$salesModel=Mage::getModel("sales/order");
$salesCollection = $salesModel->getCollection();

foreach($salesCollection as $order)
{
    $orderId= $order->getIncrementId();
    echo $orderId;
}