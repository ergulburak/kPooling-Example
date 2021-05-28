# kPooling-Example

Eğer önceden pooling sistemi hazırlamadıysak jamlerde yada küçük çaplı bireysel projelerimizde büyük optimizasyon sorunları yaşayabiliriz. Böyle durumlarda elimizin altında **kPooling** paketinin olması vakit kazandıracaktır.

kPooling, Unity için bir object-pooling sistemidir. Flexible generic tip API'ye dayanır ve varsayılan olarak `GameObject` türü poolların oluşturulmasını ve yönetilmesini destekler. kPooling ayrıca, hem runtime hem de editör'de C# tipi pooling desteğini basit ama kuvvetli `İşlemci` API'ı ile birlikte gelir.

## Özellikleri

* Herhangi bir C# nesnesi için Flexible pooling API
* Varsayılan olarak GameObject desteği
* Custom pool türü yönetimi için API

## Kurulumu

* `ProjeDizini/Packages/manifest.json` dizinindeki manifest dosyasını açın.
* İçerisinedeki `dependencies` listesine `"com.kink3d.pooling": "https://github.com/Kink3d/kPooling.git"` satırını ekleyin.
* Kayıt edip kapattıktan sonra projeyi açın.

![resim1](https://ergulburak.github.io/assets/img/kPooling-Example/kPool-2.PNG)

## Kullanımı

* **CreatePool** (Havuz oluşturma)

Havuz oluşturmak için 3 şeye ihtiyacınız var. Bunlar: Ana obje, Anahtar obje, Havuzdaki obje sayısı.
Örneğimde mermiler için kullanmaya karar verdim. Ana obje ve anahtar obje için mermi prefabini kullanabiliriz. Anahtarla ana obje aynı olabilir. Değişkenlerimiz ve oluşturma metodu resimdeki gibidir:

![resim1](https://ergulburak.github.io/assets/img/kPooling-Example/kPool-3.PNG)

* **TryGetInstance** (Havuzdan nesne çağırma)

Bu metodu kullana bilmek için iki değişkene ihtiyacımız var. Birincisi anahtar, ki anahtarı az önce oluşturduk. İkincisi de çağırılan objenin yazılacağı değişen, onu da önceki resimde instanceProjectile olarak tanımladık. Metod bittikten sonra aşağıdaki gibi gözükecek:

![resim1](https://ergulburak.github.io/assets/img/kPooling-Example/kPool-4.PNG)

* **ReturnInstance** (Nesneyi geri gönderme)

Mermi ile işimiz bittikten sonra havuza tekrar geri yollamak için bu metodu kullanıyoruz. Yine metodu kullanabilmek için anahtara ve göndereceğimiz objeye ihtiyacımız var. Mermi ömrünü tamamladığında aşağıdaki gibi geri havuza gönderiliyor:

![resim1](https://ergulburak.github.io/assets/img/kPooling-Example/kPool-5.PNG)

Kaynak için [buraya][kaynak] tıklayınız.

[git]: https://github.com/ergulburak/kPooling-Example
[kaynak]: https://github.com/Kink3d/kPooling
