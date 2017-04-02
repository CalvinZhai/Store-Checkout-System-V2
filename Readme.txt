0. Application's business requirements: ...\Doc\....pdf

2. Visual Studio solution file: 
	> GroceryCo.Checkout.vs2010.sln: need Microsoft Visual Studio 2010 and .NET Framework 4.0
    > NUnit version: NUnit-3.6.1

3. How to run and test the application:
    > Environment: VS2010, .NET Framework 4.0 and ...\Lib\NUnit-2.6.4
    > Download to local folder and unzip if needed 
	> Double click GroceryCo.Checkout.vs2010.sln located at root to open
    > Two ways to test:
        > Checkout.Console project: 
            > Set it as start-up project and press F5, or select the project > right click > Debug > Start new instance
            > The console app will load 2 files from ...\Data\ folder: Data.Basket.xml and Data.ProductCatalog.xml
            > The above 2 .xml files' path are configured in app.config file: "ProductCatalogProxyUri" and "BasketUri"
            > The app will generate a Order object, do the pricing work by applying promotions, generate the Receipt 
                and finally ouput the receipt to console and ...\Src\Console\bin\Debug\OrderReceipt.Output.txt file
            > You can modify the 2 .xml files' data and run the app again and again to verify things

        > Checkout.UnitTest project(NUnit test cases...)
            > Select the project > contex menu: Properties > Debug
                > Start Action > ensure Start external program... selected and point to ...\Lib\NUnit-2.6.4\nunit.exe file
                > Start Options: ensure Command line arguments is: GroceryCo.Checkout.UnitTest.dll
            > Set it as start-up project and press F5 to launch NUnit GUI which will automatically load all the test cases
            > This project contains 35 around test cases, covering different scenarios:
                basket data load, catalog data load, normal flow, negative flow, corner cases, validation, 
                single promotion per file, combined promotions, add the ultimate case "TestEnd2EndMultiProductsAllPromotions"
            > Pure API based test cases (e.g. in TestMultipleProductPromotions.cs) and xml data-driven test cases (e.g.TestEnd2EndMultiProductsAllPromotions.cs) 
            > TestEnd2EndMultiProductsAllPromotions: this is the one you may want to check into...
            > Testing data located at ..\Data\ folder under the project, folders under it are named properly with respects to test class file, 
                 test data used by TestEnd2EndMultiProductsAllPromotions.cs is right put under folder Data\End2EndMultiProductsAllPromotions
            > Do NOT tough testing data otherwise, unit tests might fail (as expected ^_^)
            > NOTE: if wanting to debug in unit test mode: F5 to run > Tools > Attach to Processes... attach ALL the "nunit-agent.exe"

        > Basket data in ...Basket.xml(more useful comments can be found inside): 
            > Customer, Customer Name and Customer Address are optional and can be empty -- additional sub-nodes e.g. <Email> will be ignored
                        <Customer>
                            <Name>Calvin Zhai</Name>
                            <Address>123 45 Street SW Calgary</Address>
                        </Customer>
            > Product items: use <Items>, <Item> and only <Name> property is required
                > product name here in basket is case INSENSITIVE,so, "apple" == "Apple" == "APPLE"
                > product <Name> is mandatory and can NOT be empty
                    <Items>
                        <Item>
                            <Name>Apple</Name>
                        </Item>
                        <Item>
                            <Name>Banana</Name>
                        </Item>
                        <Item>
                            <Name>Orange</Name>
                        </Item>
                    </Items>
                > if no matching product found in catalog, customized exception thrown up
        
        > Catalog data in ...ProductCatalog.xml(more useful comments can be found inside):
            > Use <Items>, <Item> to represent products
            > Each product can be presented by one and ONLY one <Item> -- duplicates will cause meaningful(^_^) exception thrown up
            > Item's name is case INSENSITIVE, so "apple" = "Apple" = "APPLE"
            > Item <Name> is mandatory and can NOT be empty
            > <Price> represent's product's regular price, mandatory and decimal expected
            > More specific Promotion related explainations can be found in following sections

        > Product name in basket.xml and catalog.xml both case insensitive, so product name in Order and Receipt all take UPPER CASE

4. (Product level)Promotions explaination(complete schema can be found at Checkout.Console project's Data\Data.ProductCatalog.xml file):
    > OnSalePricedPromotion.cs:     <Promotion Name="OnSalePriced" Rule=".8"/>,             product on sale at $0.8
    > OnSaleOffPromotion.cs:        <Promotion Name="OnSaleOff" Rule="40"/>                 product on sale with 40% off
    > GroupPricedPromotion.cs:      <Promotion Name="GroupPriced" Rule="3-2.0"/>            buy 3 at $2.0
    > GroupAdditionFreePromotion.cs:<Promotion Name="GroupAdditionFree" Rule="3-2"/>        buy 3 get 2 for free
    > GroupAdditionOffPromotion.cs: <Promotion Name="GroupAdditionOff"  Rule="3-2-50"/>     buy 3 get 2 for 50% off

    > Promotion class naming convention: has to end with word "Promotion"
    > Promotion Name attribute's value in catalog file must NOT end with word "Promotion"

    > No promotion stacking: so when multiple promotion applied, the cheapest one would be picked and used to do the calculation.
        Product.Promotions holds all the possibly applicable promotions for the product (defined in product catalog xml file)
        OrderItem.AppliedPromotion holds the one (maximum) which actually get applied to the item

    > OnSaleOffPromotion.cs: NOT actually required by the story, just used to show the extensibility. 
        UnitTests: TestOnSaleOffPromotion.cs and TestEnd2EndMultiProductsAllPromotions.cs 

5. Additional Notes:
        > Xml parsers: Xml to LINQ has wired file location issue, so the app is using normal Xml API - small thing anyway ^_^
        > RuleModel: slightly different naming convention for it's members than C# standard. More like JavaScript world. 
            The reason behind is to make promotion rule factors as close to humanbeing's language as possible
        > XML data: major data validation being implemented, however, please supply with valid data, otherwise, console app may break.
        > All Promotions' .Rule has been validated properly. Meaningful exceptions being thrown if rule value being set unexpectedly.
            Implemented by promotions class xxxPromotion.ValidateRule()
        > Some classes/functions implemented without being used yet like things in Framework and Order level promotion interfaces/base classes
            Maybe useful in future...
        > Thinking to commit a few order-level promotions e.g. "VIP client will get additional 5% off" 
            and "City Employee will get additional 10% off" (real story from Mark's^_^). 
            Have basic interfaces/classes implemented but without further implementation so far as time limited.
            Should not be a big deal, only time matters...
        > Unit test codes may not be well engineered as time limited but functionality wise should provide with reasonable coverages

Please let me know right away if you are experiencing issues and/or problems by mail at calvin.cagm@gmail.com
																																							
Thanks,																		
Calvin Zhai, 2017.03.23 - 2017.03.30